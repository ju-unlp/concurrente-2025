/*
En una oficina existen 100 empleados que envían documentos para imprimir en 5 impresoras compartidas. Los pedidos de impresión son procesados por orden de llegada y se asignan a la primera impresora que se encuentre libre. Implemente un programa que permita resolver el problema anterior usando PASAJE DE MENSAJES ASINCRÓNICO (PMA).
*/

chan solicitudImpresion(int, String);
chan impresion[100](String);

Process Empleado[id: 1..100]{
    String doc, imp;
    while true {
        generarDocumento(doc);
        send solicitudImpresion(id, doc);
        receive impresion[id](imp);
    }
}

Process Impresora[id: 1..5]{
    int auxId;
    String doc, imp;
    while true {
        receive solicitudImpresion(auxId, doc);
        imp = imprimir(doc);
        send impresion[auxId](imp);
    }
}

// MAL - NO ES NECESARIO QUE EL EMPLEADO HAGA EL IMPRIMIR Y QUE SEPA LA IMPRESORA 

chan impresoraLibre(int);
chan pedidoImpresora(int);
chan liberarImpresora(int);
chan permisoImprimir(int)[100];

Process Empleado[id: 1..100]{
    int impresora;
    while true {
        send pedidoImpresora(id);
        rececive permisoImprimir(impresora);
        imprimir(impresora);
        send liberarImpresora(impresora);
    }
}

Process Coordinador{
    queue libres = {1, 2, 3, 4, 5};
    int auxID, auxImp; 
    do 
        when (empty(librerarImpresora) and not empty(libres)) -->
            accept pedidoImpresora(id) do
                auxId = id;
                pop(libres, auxImp);
                send permisoImprimir[auxId](auxImp);
            end pedidoImpresora;
    [] accept liberarImpresora(auxImp) do
            push(libres, auxImp);
        end liberarImpresora;
}