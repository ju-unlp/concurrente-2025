/*
En una oficina existen 100 empleados que envían documentos para imprimir en 5 impresoras compartidas. Los pedidos de impresión son procesados por orden de llegada y se asignan a la primera impresora que se encuentre libre. Implemente un programa que permita resolver el problema anterior usando PASAJE DE MENSAJES SINCRÓNICO (PMS).
*/

Process Empleado[id: 1..100]{
    String documento;

    while true{
        generarDocumento(documento);
        Coordinador!pedidoImpresion(documento);
    }
}

Process Coordinador{
    String documento;
    queue documentos;
    int impId;

    while true{
        if Empleado[*]?pedidoImpresion(documento) -->
            push(documentos, documento);
        [] (not empty(documentos)) Impresora[*]?libre(impId) -->
            pop(documentos, documento);
            Impresora[impId]!archivoImpresion(documento);
    }
}

Process Impresora[id: 1..5]{    
    String documento;

    while true{
        Coordinador!libre(id);
        Coordinador?archivoImpresion(documento);

        imprimir(documento);
    }
}