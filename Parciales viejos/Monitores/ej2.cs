/*
Resolver con MONITORES la siguiente situación. Simular la atención en un corralón de materiales donde hay 4 empleados para atender a N clientes de acuerdo al orden de llegada. Cuando el cliente llega espera a que alguno de los empleados lo llame, se dirige hasta el escritorio del mismo y le entrega el listado de materiales que debe comprar, luego queda esperando a que terminen de atenderlo y el empleado le entregue la factura. 
Nota: maximizar la concurrencia; suponga que existe una función HacerFactura()llamada por el empleado que simula la atención.

*/

Monitor Corralon{
    cond clientes, empleados;
    queue filaCli;
    int empAsignado[N];

    Procedure llegada(int id, OUT int idEmp){
        // me sumo a la fila
        push(filaCli, id);
        // despierto al empleado
        signal(empleados);
        // me duermo
        wait(clientes);
        // busco mi empleado
        idEmp = empAsignado[id];
    }

    Procedure Siguiente(int id){
        int idCli;
        while (empty(clientes)){
            wait(empleados);
        }
        pop(filaCli, idCli);
        empAsignado[idCli] = id;
    }

}

Monitor Escritorio[id: 1..4]{
    bool listo = false;
    String pedido, factura;
    cond cliente, emp;

    Procedure atencion(String p, OUT String f){
        // guarda el pedido
        pedido = p;
        // avisa que esta el pedido
        listo = true;
        // despierta al empleado
        signal(emp);
        // espera
        wait(cliente);
        // agarra factura
        f = factura;
        // despierta empleado
        signal(emp);
    }

    Procedure verPedido(OUT String p){
        // si emp no dio pedido se duerme
        if(not listo){
            wait(emp);
        }
        // actualiza listo pal prox
        listo = false;
        // agarra pedido
        p = pedido;
    }

    Procedure darFactura(String f){
        // guarda factura
        factura = f;
        // despieta cliente
        signal(cliente);
        // espera que cliente agarre factura
        wait(emp);
    }

}

Process Cliente[id: 1..N]{
    int idEmp;
    String pedido, factura;

    Corralon.llegada(id, idEmp);
    Escritorio[idEmp].atencion(pedido, factura);
}

Process Empleado[id: 1..4]{
    String factura, pedido;
    while true{
        Corralon.siguiente(id);
        Escritorio[id].verPedido(pedido);
        hacerFactura(pedido, factura);
        Escritorio[id].darFactura(factura);
    }
}