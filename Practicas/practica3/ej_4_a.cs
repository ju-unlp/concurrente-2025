/*
En un corralón de materiales se deben atender a N clientes de acuerdo con el orden de llegada. Cuando un cliente es llamado para ser atendido, entrega una lista con los productos que comprará, y espera a que alguno de los empleados le entregue el comprobante de la compra realizada.
    a. Implemente una solución considerando que el corralón tiene un único empleado y cada cliente hace un único pedido. El empleado debe terminar su ejecución.
*/

Monitor Corralon{
    int esperando = 0;
    int clientes = N;
    cond cola;
    cond emp;
    cond cliente;
    String pedido;
    String comprobante;

    Procedure arribo(){
        /* Cliente llega, despierta empleado, se duerme */
        signal(emp);
        esperando ++;
        wait(cola);
    }

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

    Procedure entregarComprobante(c> IN String){
        /* Cliente entrega comprobante */
        comprobante = c;
        signal(cliente);
    }

    Procedure siguiente(terminar: OUT bool){
        /* Si hay cliente en la fila, lo despierta y chequea si es el último, dsp se duerme */
        if(esperando == 0){
            wait(cliente);
        }
        esperando--;
        signal(cola);
        clientes--;
        if (clientes == 0) {
            terminar = true;
        } else {
            terminar = false;
        }
    }

    Procedure atencion(p: OUT String){
        /* Empleado recibe el pedido */
        p = pedido;
    }

}
Process Empleado{
    bool terminar = false;
    String pedido;
    String comprobante;

    while (not terminar) {
        Corralon.siguiente(terminar);
        Corralon.atencion(pedido);
        // hacerPedido(pedido);
        // hacerComprobante(comprobante);
        Corralon.entregarComprobante(comprobante);
    }
}

Process Cliente[id: 0..N]{
    String pedido;
    String comprobante;

    // avisa que llegó
    Corralon.arribo();
    // da el pedido
    Corralon.pedido(pedido);
    // espera el comprobante
    Corralon.recibirComprobante(comprobante);
}