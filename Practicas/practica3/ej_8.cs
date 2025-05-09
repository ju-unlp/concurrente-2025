/*
En un examen de la secundaria hay un preceptor y una profesora que deben tomar un examen escrito a 45 alumnos. El preceptor se encarga de darle el enunciado del examen a los alumnos cundo los 45 han llegado (es el mismo enunciado para todos). La profesora se encarga de ir corrigiendo los exámenes de acuerdo con el orden en que los alumnos van entregando. Cada alumno al llegar espera a que le den el enunciado, resuelve el examen, y al terminar lo deja para que la profesora lo corrija y le envíe la nota. 

Nota: maximizar la concurrencia; todos los procesos deben terminar su ejecución; suponga que la profesora tiene una función corregirExamen que recibe un examen y devuelve un entero con la nota.
*/

Process Alumno[id: 0.. 44]{
    examen exam;
    String enunciado;
    int nota;

    Preceptor.llegar();
    Preceptor.entregarEnunciado(enunciado);
    /* hace examen */
    Examen.entregar(id, examen);
    Examen.recibirNota(nota);
}

Process Profesora{
    examen exam;
    int nota;
    int alumno;

    for( int i = 0 to 44){
        Examen.recibir(alumno, exam);
        nota = corregirExamen(examen);
        Examen.entregarNota(alumno, nota);
    }   
}

Monitor Preceptor{
    String enunciado = "lorem ipsum";
    int total;
    cond estudiantes;

    procedure.llegar(){
        total ++;
        if(total == 45){
            signal_all(estudiantes);
        } else {
            wait(estudiantes);
        }
    }

    procedure entregarEnunciado(en: out String){
        en = enunciado;
    }
}

Monitor Examen{
    cond prof;

    int cantEsperandoCorreccion = 0;
    cond esperandoCorreccion[44];
    cola estudiantes;

    int notas[44];

    procedure entregar(id: in int, exam: in examen){
        estudiantes.push(id, exam);
        cantEsperandoCorreccion ++;
        signal(prof);
        wait(esperandoCorreccion[id]);
        
    }

    procedure recibir(alumno: out int, exam: out examen){
        if(cantEsperandoCorreccion == 0){
            wait(prof);
        }
        alumno, exam = estudiantes.pop(id, exam);
    }

    procedure entregarNota(alumno: in int, nota; in int){
        notas[alumno] = nota;
        cantEsperandoCorreccion --;
        signal(esperandoCorreccion[alumno]);
    }

    procedure recibirNota(id: in int, nota: out int){
        nota = notas[id];
    }

}
