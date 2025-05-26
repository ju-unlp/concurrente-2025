/*
Se debe modelar el funcionamiento de una casa de comida rápida, en la cual trabajan 2 cocineros y 3 vendedores, y que debe atender a C clientes. El modelado debe considerar que:
- Cada cliente realiza un pedido y luego espera a que se lo entreguen.
- Los pedidos que hacen los clientes son tomados por cualquiera de los vendedores y se lo pasan a los cocineros para que realicen el plato. Cuando no hay pedidos para atender, los vendedores aprovechan para reponer un pack de bebidas de la heladera (tardan entre 1 y 3 minutos para hacer esto).
- Repetidamente cada cocinero toma un pedido pendiente dejado por los vendedores, lo cocina y se lo entrega directamente al cliente correspondiente.

Nota: maximizar la concurrencia.
*/
chan esperarAtencion(int); // idCliente
chan recibirAtencion[C](int) // idVendedor

chan buscarCliente(int); // idVendedor
chan siguente[3](int); // idCliente

chan pedidosAVendedor[3](String); // pedido
chan pedidosACocina(int, String); // idCliente, pedido

chan plato[C](bool); 

chan 
Process Cliente[id: 0..C-1]{
    bool llego;
    int vendedor;
    String pedido = 'carne con papas';
    // espero que me atiendan
    send esperarAtencion(id);
    receive recibirAtencion(vendedor);
    // llegó el mozo a atenderme
    send pedidosAVendedor[vendedor](pedido);
    // espero mi plato
    receive plato[id](llego);
    // come plato yay
}

Process Vendedor[id: 0..2]{
    int cliente;
    String pedido;
    while true{
        // voy a ver si hay un cliente
        send buscarCliente(id);
        receive siguiente[id](cliente);
        // si hay cliente lo atiendo
        if(cliente <> -1){
            send recibirAtencion[cliente](id);
            receive pedidosAVendedor[id](pedido);
            send pedidosACocina(cliente, pedido);
        } else {
            // no hay cliente
            // voy a buscar bebidas
            delay(300);
        }
    }

}

Process Coordinador{
    int cliente, vendedor;
    while true{
        receive buscarCliente(vendedor);
        // si hay cliente esperando se lo mando
        if (not empty(esperarAtencion)) {
            receive esperarAtencion(cliente);
            send siguiente[vendedor](cliente);
        } else {
            // sino lo mando a buscar botellas
            send siguiente[vendedor](-1);
        }
    }
}

Process Cocinero[id: 0..1]{
    int cliente;
    String pedido;

    while true{
        receive pedidosACocina(cliente, pedido);
        // cocina plato
        cocinar(pedido); // asumimos que existe funcion
        // manda plato a cliente
        send plato[cliente](true);
    }
}
