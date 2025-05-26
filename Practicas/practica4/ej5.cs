/*
Resolver la administración de las impresoras de una oficina. Hay 3 impresoras, N usuarios y 1 director. Los usuarios y el director están continuamente trabajando y cada tanto envían documentos a imprimir. Cada impresora, cuando está libre, toma un documento y lo imprime, de acuerdo con el orden de llegada, pero siempre dando prioridad a los pedidos del director. 
Nota: los usuarios y el director no deben esperar a que se imprima el documento.
*/

chan pedidos(int, tipo_archivo); // idCliente
chan pedidosDir(tipo_archivo);

chan impresion[N](tipo_impresion);
chan impresionDir(tipo_impresion);

chan pedirArchivo(int); // idImpresora
chan siguiente[2](int, archivo); // idCliente

Process Usuario[id: 0..N-1]{   
    tipo_archivo archivo;
    tipo_impresion impresion;
    while true{
        send pedidos(id, archivo);
        recieve impresion[id](impresion);
    }
}

Process Director{
    tipo_archivo archivo;
    tipo_impresion impresion;
    while true{
        send pedidosDir(archivo);
        recieve impresionDir(impresion);
    }
}

Process Coordinador{
    int impresora, cli;
    tipo_archivo archivo;
    while true{
        // espero a que haya impresora libre
        receive pedirArchivo(impresora);
        do
            (not empty(pedidosDir)) ->
                //si el director pidio algo
                receive pedidorDir(archivo);
                send siguiente[impresora](-1, archivo);
            (empty(pedidorDir) and not empty(pedidos)) ->
                receive pedidos(cli, archivo);
                send siguiente[impresora](cli, archivo);
        end do
    }

}

Process Impresora[id: 0..2]{
    int cli;
    tipo_archivo archivo;
    while true{
        send pedirArchivo(id);
        recieve siguiente[id](cli, archivo);
        // imprimir
        Imprimir(archivo, impresion);
        if( cli == -1 ){
            send impresionDir(impresion);
        } else {
            send impresion[cli](impresion);
        }
    }
}