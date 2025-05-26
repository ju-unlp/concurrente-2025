/*
Se desea modelar el funcionamiento de un banco en el cual existen 5 cajas para realizar pagos. Existen P clientes que desean hacer un pago. Para esto, cada una selecciona la caja donde hay menos personas esperando; una vez seleccionada, espera a ser atendido. En cada caja, los clientes son atendidos por orden de llegada por los cajeros. Luego del pago, se les entrega un comprobante. 
Nota: maximizando la concurrencia.
*/

chan solicitarCaja(int); // int = idCliente
chan solicitarAtencion[5](int); // int = idCliente
chan recibirAtencion[P](int); // int = idCaja
chan solicitarComprobante[5](bool);
chan recibirComprobante[P](String);
chan terminarAtencion(int); // int = idCaja

Process Persona [id: 0..P-1]{
    int caja;
    String comprobante;

    send solicitarCaja(id);
    receive recibirAtencion[id](caja);
    // usar caja
    send solicitarComprobante[caja](true);
    receive recibirComprobante[id](comprobante);
}

Process Coordinador{
    int cli, caja;
    int cantEspera[5] = ([5] = 0);
    int min;
    while true {
        /* actualiza la cola de espera */
        while( not empty(terminarAtencion)){
            int c;
            receive terminarAtencion(c);
            cantEspera[c]--;
        }

        receive solicitarCaja(cli);
        /* funcion recorre arreglo cantEspera y devuelve en min la posicion cuyo valor sea menor. En caso de tener más de una opción igual retorna uno de forma no determinística */
        calcularMinCaja(min, cantEspera);
        cantEspera[min]++;
        send solicitarAtencion[min](cli);
    }
}

Process Caja[id: 0..4]{
    int cli;
    String comprobante;
    bool seguir;
    while true{
        receive solicitarAtencion[id](cli);
        send recibirAtencion[cli](id);
        // persona usa caja
        receive solicitarComprobante[id](seguir);
        generarComprobante(comprobante); // damos por sentado que existe
        send recibirComprobante[cli](comprobante);

        send terminarAtencion(id); // aviso a coordinador que me liberé
    }
}