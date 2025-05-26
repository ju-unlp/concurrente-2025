/*
Simular la atención en un locutorio con 10 cabinas telefónicas, el cual tiene un empleado que se encarga de atender a N clientes. Al llegar, cada cliente espera hasta que el empleado le indique a qué cabina ir, la usa y luego se dirige al empleado para pagarle. El empleado atiende a los clientes en el orden en que hacen los pedidos, pero siempre dando prioridad a los que terminaron de usar la cabina. A cada cliente se le entrega un ticket factura. 
Nota: maximizar la concurrencia; 
    suponga que hay una función Cobrar() llamada por el empleado que simula que el empleado le cobra al cliente.
*/

chan esperarAtencion(int); // idCliente
chan esperarTicket(int); // idCliente
chan cabinaLibre(int); // nroCabina

chan recibirCabina[N](int); // nroCabina
chan recibirTicket[N](String); // ticket

Process Cliente[id: 0..N-1]{
    int cabina;
    String ticket;
    // se suma a la cola
    send esperarAtencion(id); 
    // espera nro cabina
    recieve recibirCabina[id](cabina);
    // usa cabina
    // cola ticket
    send cabinaLibre(cabina);
    send esperarTicket(id);
    // recibir ticker
    recieve recibirTicket[id](ticket);
}

Process Empleado{
    int cliente, cabina;
    String ticket;
    while true{
        do
            // si hay alguien en cola ticket atiende a ese
            (not empty(esperarTicket)) ->
                recieve esperarTicket(cliente);
                Cobrar(cliente, ticket);
                send recibirTicket[cliente](ticket);
            // si no hay gente esperando ticket & hay cabina libre & hay gente esperando cabina
            (empty(esperarTicket) and not empty(cabinaLibre) and not empty(esperarAtencion)){
                recieve cabinaLibre(cabina);
                recieve esperarAtencion(cliente);
                send recibirCabina[cliente](cabina);
        end do
    }
}