/*
Resolver con SEMÁFOROS el siguiente problema. Se debe simular el uso de una máquina expendedora de gaseosas con capacidad para 100 latas por parte de U usuarios. Además existe un repositor encargado de reponer las latas de la máquina. Los usuarios usan la máquina según el orden de llegada. Cuando les toca usarla, sacan una lata y luego se retiran. En el caso de que la máquina se quede sin latas, entonces le debe avisar al repositor para que cargue nuevamente la máquina en forma completa. Luego de la recarga, saca una lata y se retira. Nota: maximizar la concurrencia; mientras se reponen las latas se debe permitir que otros usuarios puedan agregarse a la fila.
*/
int cantLatas = 100;
sem turno[U] = ([U]0);
queue fila;
sem mutex = 1;
sem necesitaRepuesto = 0;
sem latasRepuestas = 0;

/* corregido */
Process Usuario[id: 0.. U-1]{
    // ir a la fila
    P(mutex);
    if(if not libre){
        push(fila, id);
        V(mutex);
        P(turno[id]);
    } else {
        libre = false;
        V(mutex);
    }
    // si no hay latas, reponer
    if(cantLatas > 0){
        V(necesitaRepuesto);
        P(latasRepuestas);
    }
    
    // saco lata
    cantLatas --;

    // siguiente
    P(mutex);
    if(not empty(fila)){
        int auxId;
        pop(fila, auxId);
        V(turno[auxId]);
    } else {
        libre = true;
    }
    V(mutex);
}

Process Repositor{
    while true{
        P(necesitaRepuesto);
        reponerLatas();
        cantLatas = 100;
        V(latasRepuestas);
    }
}

// en la version corregida usan un booleano para libre y un solo mutex
// el repositor es igual
// version corregida

