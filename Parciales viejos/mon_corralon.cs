// no está bien esta porque el coso del escritorio
/*
Resolver con MONITORES la siguiente situación. Simular la atención en un corralón de materiales donde hay 4 empleados para atender a N clientes de acuerdo al orden de llegada. Cuando el cliente llega espera a que alguno de los empleados lo llame, se dirige hasta el escritorio del mismo y le entrega el listado de materiales que debe comprar, luego queda esperando a que terminen de atenderlo y el empleado le entregue la factura. 
Nota: maximizar la concurrencia; suponga que existe una función HacerFactura() llamada por el empleado que simula la atención.
*/

Processs Cliente[id: 0 to N-1]{
    String pedido;
    String factura;

    // ponerse en la fila
    Corralon.llegar(id);
    // esperar turno y entregar materiales
    Corralon.entregarPedido(id, pedido);
    // esperar factura e irse
    Corralon.recibirFactura(id, factura);

}

Process Empleado[id: 0 to 3]{
    String pedido;
    String factura;
    int idCliente;

    while(true){
        // esperar cliente y guardar listado
        Corralon.atenderCliente(idCliente, pedido);
        // generar factura a partir de listado
        factura = HacerFactura(pedido);
        // subir factura
        Corralon.entregarFactura(idCliente, factura);
    }
}

Monitor Corralon{
    int cantClientes = 0;
    queue colaClientes;
    cond turno[N];

    cond empleados;
    int empleadosLibres = 4;

    String pedidos[N];
    String facturas[N];

    procedure llegar(clienteId: in int){
        cantClientes ++;
        push(colaClientes, clienteId);

        // si hay por lo menos un empleado libre despierto a uno
        if(empleadosLibres > 0){
            signal(empleados);
        }
        wait(turno[clienteId]);
    }

    procedure antenderCliente(clienteId: out int, p: out String){
        while(cantClientes == 0){
            wait(empleados);
        }

        // saco el id de la cola y despierto el cliente correspondiente
        pop(colaClientes, clienteId);
        signal(turno[clienteId]);

        // no estoy disponible
        empleadosLibres --;
        cantClientes --;
    }

    procedure entregarPedido(id: in int, p: in String){
        pedidos[id] = p;
        /* ERROR: tiene que saber a quien despertar */
        wait(turno[id]);
    }

    procedure entregarFactura(clienteId: in int, f: in int){
        facturas[clienteId] = f;
        empleadosLibres ++;
        signal(turno[N]);
    }

    procedure recibirFactura(id: in int, f: out String){
        f = facturas[id];
    }

}