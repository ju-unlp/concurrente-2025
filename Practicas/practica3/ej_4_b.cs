/*
En un corralón de materiales se deben atender a N clientes de acuerdo con el orden de llegada. Cuando un cliente es llamado para ser atendido, entrega una lista con los productos que comprará, y espera a que alguno de los empleados le entregue el comprobante de la compra realizada.
    b. Implemente una solución considerando que el corralón tiene E empleados (E > 1). Los empleados no deben terminar su ejecución.
*/

Monitor Corralon{
    int esperando = 0;
    cond cola;
    cond empleados;
    cola empLibres;

    Procedure arribo(){
        esperando++;
        wait(cola);
        signal(empleados);
    }

    Procedure asignarTurno(emp: OUT int){
        /* Asigna empleado para un cliente y lo despierta? */
        emp = empLibres.pop();
    }

    Procedure siguiente(empId: in int){
        /* Si hay cliente en la fila, lo despierta, dsp se duerme */
        empLibres.push(empId);
        while(esperando == 0) {
            wait(empleados);
        }
        esperando--;
        signal(cola);
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
    String pedido;
    String comprobante;

    while (true) {
        Corralon.siguiente(id);
        Atencion[id].atencion(pedido);
        // hacerPedido(pedido);
        // hacerComprobante(comprobante);
        Atencion[id].entregarComprobante(comprobante);
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