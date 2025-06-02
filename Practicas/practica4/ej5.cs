/*
Resolver la administración de las impresoras de una oficina. Hay 3 impresoras, N usuarios y 1 director. Los usuarios y el director están continuamente trabajando y cada tanto envían documentos a imprimir. Cada impresora, cuando está libre, toma un documento y lo imprime, de acuerdo con el orden de llegada, pero siempre dando prioridad a los pedidos del director. 
Nota: los usuarios y el director no deben esperar a que se imprima el documento.
*/

chan pedidos(tipo_archivo); // idCliente
chan pedidosDir(tipo_archivo);

chan pedirArchivo(int); // idImpresora
chan siguiente[2](archivo); // idCliente

Process Usuario[id: 0..N-1]{   
    tipo_archivo archivo;
    while true{
        send pedidos(archivo);
    }
}

Process Director{
    tipo_archivo archivo;
    while true{
        send pedidosDir(archivo);
    }
}

Process Coordinador{
    int impresora;
    tipo_archivo archivo;
    while true{
        do
            (not empty(pedidosDir)) ->
                //si el director pidio algo
                receive pedidosDir(archivo);
                receive pedirArchivo(impresora);
                send siguiente[impresora](archivo);
            (empty(pedidosDir) and not empty(pedidos)) ->
                receive pedidos(archivo);
                receive pedirArchivo(impresora);
                send siguiente[impresora](archivo);
        end do
    }

}

Process Impresora[id: 0..2]{
    tipo_archivo archivo;
    while true{
        send pedirArchivo(id);
        recieve siguiente[id](archivo);
        Imprimir(archivo, impresion);
    }
}