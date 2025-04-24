/*
En un corralón de materiales se deben atender a N clientes de acuerdo con el orden de llegada. Cuando un cliente es llamado para ser atendido, entrega una lista con los productos que comprará, y espera a que alguno de los empleados le entregue el comprobante de la compra realizada.
    a. Implemente una solución considerando que el corralón tiene un único empleado y cada cliente hace un único pedido. El empleado debe terminar su ejecución.
    
    b. Implemente una solución considerando que el corralón tiene E empleados (E > 1). Los empleados no deben terminar su ejecución.
    
    c. Modifique la solución (b) considerando que los empleados deben terminar su ejecución cuando se hayan atendido todos los clientes (cada cliente hace un único pedido).
*/

Monitor Corralon{
    int clientes = N;
    cond cola;
    int esperando = 0;
    bool libre;

    Procedure arribo(){
        wait(cola);
    }

    Procedure pedido(pedido: IN String){
        // entregar pedido
    }

    Procedure recibirComprobante(comprobante: OUT String){

    }

    Procedure hacerComprobante(){

    }

    Procedure atencion(){
        // si no quedan mas por atender terminar true
    }

}
Process Empleado{
    bool terminar = false;
    String pedido;
    String comprobante;

    while (not terminar) {
        Corralon.atencion(terminar, pedido);
        // hacer comprobante
        Corralon.hacerComprobante(comprobante);
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