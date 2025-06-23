/*
Resolver con PMS (Pasaje de Mensajes SINCRÓNICOS) el siguiente problema. Hay un teléfono público que debe ser usado por U usuarios de acuerdo al orden de llegada (se debe usar con exclusión mutua). El usuario debe esperar su turno, usa el teléfono y luego lo deja para que el siguiente lo use. Nota: cada usuario usa sólo una vez el teléfono.
*/

Process Usuario[id: 1..N]{
    Telefono!Llegada(id);
    Telefono?Turno();
    Telefono!Liberar();
}

Process Telefono{
    int auxId;
    queue cola;
    bool libre = true;
    do Usuario[*]?Llegada(auxId) --> 
        if(not libre){
            push(cola,auxId);
        } else {
            libre = false;
            Usuario[auxId]!Turno(); 
        }
    [] Usuario[*]?Liberar() --> 
        if(not empty(cola)){
            pop(cola, auxId);
            Usuario[auxId]!Turno();
        } else {
            libre = true;
        }
}

// -------------------------------------------------------------------------- //
// SOLUCIÓN INCORRECTA PORQUE ESTO NO ASEGURA QUE SE ORDENA POR ORDEN DE LLEGADA 
// ADEMAS PUSE SEND Y RECEIVE DONDE NO VA

Process Usuario[id: 1..N]{
    // llega usuario
    Telefono!Llegada(id);
    // espera turno
    receive Telefono?Turno();
    // usa telefono
    // bye
    send Telefono!Liberar();
}

Process Telefono{
    int auxId;
    while true{
        // agarra usuario esperando
        receive Usuario[*]?Llegada(auxId);
        // le da telefono
        send Usuario[auxId]!Turno();
        // espera que lo termine de usar
        receive Usuario[auxId]?Liberar();
    }
}

