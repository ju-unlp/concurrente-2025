/*
Existe una comisión de 50 alumnos que deben realizar tareas de a pares, las cuales son corregidas por un JTP. Cuando los alumnos llegan, forman una fila. Una vez que están todos en fila, el JTP les asigna un número de grupo a cada uno. Para ello, suponga que existe una función AsignarNroGrupo() que retorna un número “aleatorio” del 1 al 25. Cuando un alumno ha recibido su número de grupo, comienza a realizar su tarea. Al terminarla, el alumno le avisa al JTP y espera por su nota. Cuando los dos alumnos del grupo completaron la tarea, el JTP les asigna un puntaje (el primer grupo en terminar tendrá como nota 25, el segundo 24, y así sucesivamente hasta el último que tendrá nota 1). Nota: el JTP no guarda el número de grupo que le asigna a cada alumno.
*/
Monitor Comision{
    cond jtp;

    int total = 0;

    cola colaAlumnos;
    cond alumno[50];
    int grupo[50]; 
    
    int termino[25] = ([25] = 0);
    cond grupoTerminado[25];
    cola esperandoNota;

    int notas[25];

    procedure esperarAlumnos(){
        #dormir jtp
        if(total < 50){
            wait(jtp);
        }
    }
    
    procedure agregarAFila(id: in int){
        colaAlumnos.push(id);
        total ++;
        if(total == 50){
            //si llegaron todos despierto al jtp
            signal(jtp);
        }
        wait(alumno[id]);
    }

    procedure asignarGrupo(nroGrupo: in int){
        //saco alumno
        int id = colaAlumnos.pop();

        //guardo su nro de grupo
        grupo[id] = nroGrupo;

        //lo despierto
        signal(alumno[id]);
    }

    procedure recibirGrupo(id: in int, nro: out int){
        //me agarro el grupo que me asignaron
        nro = grupo[id];
    }

    procedure terminarTarea(id: in int, grupo: in int){
        //sumo uno al termine grupo
        termino[grupo] ++;
        
        //si terminamos los dos despierto al jtp
        if(termino[grupo] == 2){
            // nos agrego a la fila de esperandoNota
            esperandoNota.push(grupo);
            signal(jtp);
        }

        //me duermo
        wait(grupoTerminado[grupo])
    }

    procedure corregirGrupo(nota: in int){
        // si no hay nadie esperando nota me voy a dormir
        while(esperandoNota.empty()){
            wait(jtp);
        }
        // agarro el grupo que termino
        int grupo = esperandoNota.pop();
        
        // guardo la nota
        notas[grupo] = nota;

        // los despierto
        signal_all(grupoTerminado[grupo]);
    }

    procedure recibirNota(nroGrupo: in int, nota: out int){
        nota = notas[nroGrupo];
    }
}

Process JTP{
    int nota = 25;

    Comision.esperarAlumnos();
    //asignar grupos
    for(int i = 0 to 49){
        Comision.asignarGrupo( asignarNroGrupo() );
    }

    for(int i = 0 to 24){
        Comision.corregirGrupo(nota);
        nota --;
    }
}

Process Alumno[id: 0..49]{
    int grupo;
    int nota;

    //se va a la fila
    Comision.agregarAFila(id);

    //agarra nro de grupo
    Comision.recibirGrupo(id, grupo);

    /* hacer tarea */

    Comision.terminarTarea(id, grupo);

    //recibir nota
    Comision.recibirNota(grupo, nota);
    
}