/*
Resolver con PASAJE DE MENSAJES ASINCRÓNICO (PMA) el siguiente problema. En un negocio de cobros digitales hay P personas que deben pasar por la única caja de cobros para realizar el pago de sus boletas. Las personas son atendidas de acuerdo con el orden de llegada, teniendo prioridad aquellos que deben pagar menos de 5 boletas de los que pagan más. Adicionalmente, las personas embarazadas tienen prioridad sobre los dos casos anteriores. Las personas entregan sus boletas al cajero y el dinero de pago; el cajero les devuelve el vuelto y los recibos de pago.
*/

chan cola(int);
chan colaEmbarazadas(int);
chan colaRapida(int);

Process Persona [id: 0..P]{
    int cantBoletas = n;
    queue boletas;
    bool embarazada: true|false;
    int monto, vuelto;
    String boleta, recibos;

    if(embarazada){
        send colaEmbarazadas(id);
    } else if (cantBoletas < 5){
        send colaRapida(id);
    } else {
        send cola(id);
    }

    receive atencion[id]();
    send pago(boleta, monto);
    receive cobro(recibos, vuelto);
}

Process Caja{
    int auxId, monto, vuelto;
    String boleta, recibos;

    while true{
        if (not empty(colaEmbarazadas)) -->
            receive colaRapida(auxId);
        [] (empty(colaEmbarazadas) and not empty(colaRapida)) -->
            receibe colaRapida(auxID);
        [] (empty(colaEmbarazadas) and empty(colaRapida) and not empty(cola)) -->
            receive cola(auxId);
    }

    send atencion[auxId]();
    receive pago(boleta, monto);
    vuelto = getTotal(boleta) - monto;
    generarRecibos(boleta, recibo);

    send cobro(recibos, vuelto);
}