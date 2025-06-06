/*
Suponga que existe un antivirus distribuido que se compone de R procesos robots Examinadores y 1 proceso Analizador. Los procesos Examinadores están buscando continuamente posibles sitios web infectados; cada vez que encuentran uno avisan la dirección y luego continúan buscando. El proceso Analizador se encarga de hacer todas las pruebas necesarias con cada uno de los sitios encontrados por los robots para determinar si están o no infectados.
    a. Analice el problema y defina qué procesos, recursos y comunicaciones serán necesarios/convenientes para resolver el problema.
    b. Implemente una solución con PMS sin tener en cuenta el orden de los pedidos.
    c. Modifique el inciso (b) para que el Analizador resuelva los pedidos en el orden en que se hicieron.
*/

// Procesos: Analizador y R Examinador

Process Analizador{
    String direccion;
    while true{
        Examinador[*]?reporte(direccion);
        AnalizarDireccion(direccion); // asume que existe
    }
}

Process Examinador[id: 1..R-1]{
    String direccion;
    while true{
        BuscarWebInfectada(direccion); // asume que existe
        Analizador!reporte(direccion);
    }
}