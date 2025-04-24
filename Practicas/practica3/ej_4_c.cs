/*
En un corralón de materiales se deben atender a N clientes de acuerdo con el orden de llegada. Cuando un cliente es llamado para ser atendido, entrega una lista con los productos que comprará, y espera a que alguno de los empleados le entregue el comprobante de la compra realizada.
    c. Modifique la solución (b) considerando que los empleados deben terminar su ejecución cuando se hayan atendido todos los clientes (cada cliente hace un único pedido).
*/

Monitor Corralon{
    int clientes = N;
    int esperando = 0;
    cond cola;
    cond empleados;
    cola empLibres;

    Procedure arribo(){
        wait(cola);
        esperando++;
        signal(empleados);
    }

    Procedure asignarTurno(emp: OUT int){
        /* Asigna empleado para un cliente y lo despierta? */
        emp = empLibres.pop();
    }

    Procedure siguiente(empId: in int, terminar: OUT bool){
        empLibres.push(empId);
        // Mientras no haya nadie en la cola, pero no atendimos a todos los clientes
        while(esperando > 0 and clientes > 0) {
            wait(empleados); // espero
        }
        // atendimos a todos los clientes
        if(clientes == 0){
            terminar = true;
            signal_all(empleados); // los despierto a todos para que vayan a terminarse
        } else {
            // hay alguien en la cola
            terminar = false;
            clientes--;
            esperando--;
            signal(cola);
        }
    }
}

Monitor Atencion[E]{
    cond cliente;
    cond emp;
    String pedido = "";
    String comprobante = "";

    Procedure pedido(p: IN String){
        /* Cliente manda su pedido y despierta al empleado */
        pedido = p;
        signal(emp);
        wait(cliente);
    }

    Procedure recibirComprobante(c: OUT String){
        /* Cliente recibe al empleado */
        c = comprobante;
    }

        Procedure atencion(p: OUT String){
        /* Si el pedido no está me duermo, sino me guardo el pedido */
        if(pedido == "") {
            wait(emp);
        }
        p = pedido;
    }

    Procedure entregarComprobante(c: IN String){
        /* Cliente entrega comprobante */
        comprobante = c;
        signal(cliente);
        pedido = "";
        comprobante = "";
    }
}

Process Empleado[id: 0..E-1]{
    bool terminar = false;
    String pedido;
    String comprobante;

    while (not terminar) {
        Corralon.siguiente(id, terminar);
        if(not terminar) {
            Atencion[id].atencion(pedido);
            // hacerPedido(pedido);
            // hacerComprobante(comprobante);
            Atencion[id].entregarComprobante(comprobante);  
        }   
    }
}

Process Cliente[id: 0..N]{
    id empleado;
    String pedido;
    String comprobante;

    // avisa que llegó
    Corralon.arribo();
    // recibe turno
    Corralon.asignarTurno(empleado);
    // da el pedido
    Atencion[empleado].pedido(pedido);
    // espera el comprobante
    Atencion[empleado].recibirComprobante(comprobante);
}