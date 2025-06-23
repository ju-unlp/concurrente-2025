/*
Resolver con PMA (Pasaje de Mensajes ASINCRÓNICOS) el siguiente problema. Simular la atención en un locutorio con 10 cabinas telefónicas, que tiene un empleado que se encarga de atender a los clientes. Hay N clientes que al llegar esperan hasta que el empleado les indica a que cabina ir, la usan y luego se dirigen al empleado para pagarle. El empleado atiende a los clientes en el orden en que hacen los pedidos, pero siempre dando prioridad a los que terminaron de usar la cabina. Nota: maximizar la concurrencia; suponga que hay una función Cobrar() llamada por el empleado que simula que el empleado le cobra al cliente.

*/

chan PedidoAtencion(int); // id Cliente
chan PedidoPagar(int, int); // id Cliente / n cabina
chan Cabina[N](int); // n cabina
chan FinAtencion()[N];

Process Cliente[id: 1..N]{
    int cabina;
    send PedidoAtencion(id);

    receive Cabina[id](cabina);
    usarCabina(cabina);
    
    send PedidoPagar(id, cabina);
    receive FinAtencion[id]();
}

Process Empleado{
    queue cabinasLibres = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
    int auxId, auxC;
    while true {
        if 
        ( not empty(PedidoPagar)) -->
            receive PedidoPagar(auxId, auxC);
            push(cabinasLibres, auxC);
            Cobrar(auxId);
            send FinAtencion[auxId]();
        
        ( not empty(PedidoAtencion) and not empty(cabinasLibres) and empty(PedidoPagar)) -->
            rececive PedidoAtencion(auxId);
            pop(cabinasLibres, auxC);
            send Cabina[auxId](auxC);
    }
}

// SOLUCION CORRECTA //
/// Problema: puede generar busy waiting ///
/* Solución: tener un canal para solicitudes, con una variable que específique el tipo de operación, y tener una cola interna dentro de Empleado para ir guardando los pedidos */ 