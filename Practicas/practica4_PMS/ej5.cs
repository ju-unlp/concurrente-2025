/*
En un estadio de fútbol hay una máquina expendedora de gaseosas que debe ser usada por E Espectadores de acuerdo al orden de llegada. Cuando el espectador accede a la máquina en su turno usa la máquina y luego se retira para dejar al siguiente. 
Nota: cada Espectador una sólo una vez la máquina.
*/

Process Espectador[id: 0.. E-1]{
    Fila!llegar(id);
    Fila?acceso();
    // usar la maquina
    Fila!liberar();
}

Process Fila {
    int idE;
    cola fila;
    bool libre = true;

    while true {
        do Espectador[*]?llegar(idE) -->
            if libre and empty(fila) {
                libre = false;
                Espectador[idE]!acceso();
            } else {
                push(fila, idE);
            }
        □ Espectador[*]?liberar -->
            if empty(fila) {
                libre = true;
            } else {
                pop(fila, idE);
                Espectador[idE]!acceso();
            }
    }
}

