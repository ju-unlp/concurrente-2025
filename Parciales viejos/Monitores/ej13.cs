/*
Resolver con MONITORES el siguiente problema: En una elección estudiantil, se utiliza una máquina para voto electrónico. Existen N Personas que votan y una Autoridad de Mesa que les da acceso a la máquina de acuerdo con el orden de llegada, aunque ancianos y embarazadas tienen prioridad sobre el resto. La máquina de voto sólo puede ser usada por una persona a la vez. Nota: la función Votar() permite usar la máquina.
*/

// 

Monitor Mesa{
    cond personas[N], autoridad, fin;
    queue(int, int) fila;

    Procedure siguiente(){
        if( empty(fila) ){
            wait(autoridad);
        } 
        int i;
        pop(fila, i);
        signal(personas[i]);
        wait(fin);
    }

    Procedure llegada(id: in int, p: in bool){
        push(esperando, (p, id)); // inserta ordenado por prioridad
        signal(autoridad);
        wait(personas[id]);
    }

    Procedure salida(){
        signal(fin);
    }
}

Process Persona[id: 0..N-1]{
    bool prioridad = ...;
    Mesa.llegada(id, prioridad);
    Votar();
    Mesa.salida();
}

Process AutoridadDeMesa{
    while true{
        Mesa.siguiente();
    }
}