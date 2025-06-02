/*
En un laboratorio de genética veterinaria hay 3 empleados. El primero de ellos
continuamente prepara las muestras de ADN; cada vez que termina, se la envía al segundo empleado y vuelve a su trabajo. El segundo empleado toma cada muestra de ADN preparada, arma el set de análisis que se deben realizar con ella y espera el resultado para archivarlo. Por último, el tercer empleado se encarga de realizar el análisis y devolverle el resultado al segundo empleado.
*/

Process Empleado1{
    tipo_adn muestra;
    while true{
        prepararMuestra(muestra); // asume que existe
        Empleado2!muestra(muestra);
    }
}

Process Empleado2{
    tipo_adn muestra;
    tipo_set setA, setB;
    tipo_analisis resultado;
    cola sets, archivo;
    while true{
        do Empleado1?muestra(muestra) -->
            prepararSet(muestra, setA);
            push(sets, setA);
        □ not empty(sets); Empleado3?pedido() -->
            pop(sets, setB);
            Empleado3!set(setB);
        □ Empleado3?resultado(resultado) -->
            push(archivo, resultado);
    }
}

Process Empleado3{
    tipo_set set;
    tipo_analisis resultado;
    while true{
        Empleado2!pedido();
        Empleado2?set(set);
        Analizar(set, resultado);
        Empleado2!resultado(resultado);
    }
    
}