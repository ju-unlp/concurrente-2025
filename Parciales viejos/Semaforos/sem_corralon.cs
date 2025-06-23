/*
Resolver con SEMÁFOROS la siguiente situación. Simular la atención en un corralón de materiales donde hay 4 empleados para atender a N clientes de acuerdo al orden de llegada. Cuando el cliente llega espera a que alguno de los empleados lo llame, se dirige hasta el escritorio del mismo y le entrega el listado de materiales que debe comprar, luego queda esperando a que terminen de atenderlo y el empleado le entregue la factura. 
Nota: maximizar la concurrencia; suponga que existe una función HacerFactura() llamada por el empleado que simula la atención.
*/

sem hayCliente = 0;
queue colaClientes;
sem mutexClientes = 1;

sem turno[N] = ([N] = 0);
String pedidos[N];
int facturas[N];

int empleado[N];
sem listadoListo[4] = ([4] = 0);

sem mutexPedidos, mutexFacturas, mutexEmpleados = 1;

Process Empleado[id: 0 to 3]{
    int auxId;
    while(true){
        P(hayCliente);

        P(mutexClientes);
        pop(colaClientes, auxId);
        V(mutexClientes);

        // que empleado con que cliente
        P(mutexEmpleados);
        empleado[auxId] = id;
        V(mutexEmpleados);

        // llamar cliente
        V(turno[auxId]);

        // esperar listado
        P(listadoListo[id]);
        P(mutexPedidos);
        String f = HacerFactura(pedidos[auxId]);
        V(mutexPedidos);
        
        // entregar factura
        P(mutexFacturas);
        facturas[auxId] = f;
        V(mutexFacturas);

        V(turno[auxId]);
    }

}

Process Cliente[id: 0 to N-1]{
    String listaMateriales;
    String factura;

    P(mutexClientes);
    push(colaClientes, id);
    V(mutexClientes);

    V(hayCliente);

    // esperar turno
    P(turno[id]);

    // subir listado
    P(mutexPedidos);
    pedidos[id] = listaMateriales;
    V(mutexPedidos);

    // avisar a empleado
    P(mutexEmpleados);
    V(listadoListo[ empleado[id] ]);
    V(mutexEmpleados);

    // esperar factura
    P(turno[id]);

    P(mutexFacturas);
    factura = facturas[id];
    V(mutexFacturas);
}