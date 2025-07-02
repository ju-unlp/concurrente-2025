/*
Resolver el siguiente problema con MONITORES. En un examen de secundaria hay un preceptor y una profesora que deben tomar un examen escrito a 45 alumnos. El preceptor se encarga de darle el enunciado del examen a los alumnos cuando los 45 han llegado (es el mismo enunciado para todos). La profesora se encarga de ir corrigiendo los exámenes de acuerdo al orden en que los alumnos van entregando. Cada alumno al llegar espera a que le den el enunciado, resuelve el examen, y al terminar lo deja para que la profesora lo corrija y le dé la nota. 
Nota: maximizar la concurrencia; todos los procesos deben terminar su ejecución; suponga que la profesora tiene una función corregirExamen que recibe un examen y devuelve un entero con la nota.

*/
Monitor Preceptor{
    cv alumnos;
    String enunciado;
    int cantAl = 0;

    Procedure conseguirEnunciado(OUT String e){
        // aviso que llegue
        cantAl ++;
        // si no llegaron todos me duermo
        if(cantAl < 45){
            wait(alumnos);
        } else {
            // si llegaron todos despierto a todos
            signal_all(alumnos);
        }
        // recibo el enunciado
        e = enunciado;
    }

    {
        crearEnunciado(enunciado);
    }
}

Monitor Mesa{
    cv alumnos;
    cv profesora;
    String examenes[45];
    int notas[45];
    queue ordenEntrega;
    

    Procedure entregar(int id, String examen, OUT int nota){
        // entrego examen
        examenes[id] = examen;
        // me voy a la fila
        push(ordenEntrega,id);
        // despierto prof
        signal(profesora);
        // me voy a dormir
        wait(alumnos)
        // cuando me despiertan agarro mi nota
        nota = notas[id];
    }

    Procedure traerExamen(out int id, out String examen){
        // si nadie entrego me duermo
        if( empty(ordenEntrega) ){
            wait(profesora);
        }
        // busco alumno
        pop(ordenEntrega, id);
        // busco su examen
        examen = examenes[id];
    }

    Procedure entregarExamen(int id, int nota){
        // guardo la nota
        notas[id] = nota;
        // despierto alumno
        signal(alumnos);
    }

}

Process Alumno[id: 0.. 44]{
    String enunciado, examen;
    int nota;
    // espera enunciado
    Preceptor.conseguirEnunciado(enunciado);
    // resuelve el examen   
    resolver(enunciado, examen);
    // entrega examen a prof
    Mesa.entregar(id, examen, nota);
}

Process Profesora{
    int id, nota;
    String examen;
    for (int i = 0 to 44){
        Mesa.traerExamen(id, examen);
        corregirExamen(examen, nota);
        Mesa.entregarExamen(id, nota);
    }
}