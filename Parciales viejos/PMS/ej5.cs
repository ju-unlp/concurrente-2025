/*
Resolver con Pasaje de Mensajes Sincrónicos (PMS) el siguiente problema. En un torneo de programación hay 1 organizador, N competidores y S supervisores. El organizador comunica el desafío a resolver a cada competidor. Cuando un competidor cuenta con el desafío a resolver, lo hace y lo entrega para ser evaluado. A continuación, espera a que alguno de los supervisores lo corrija y le indique si está bien. En caso de tener errores, el competidor debe corregirlo y volver a entregar, repitiendo la misma metodología hasta que llegue a la solución esperada. Los supervisores corrigen las entregas respetando el orden en que los competidores van entregando. Nota: maximizar la concurrencia y no generar demora innecesaria.
*/

Process Organizador{
    int idC;
    String consiga;
    for int i 1..N {
        Competidor[*]?llegada(idC); // faltaba esto
        Competidor[idC]!Avisar(consigna); 
    }
}

Process Competidor[id: 1..N]{
    String consigna, desafio;
    bool correcto = false;

    Organizador!llegada(id); // tambien agregue esto
    Organizador?Avisar(consigna);
    
    while (not correcto){
        desafio = hacerDesafio(consigna);
        Mesa!desafioTerminado(id, desafio);

        Supervisor[*]?correccion(correcto);
    }
}

Process Mesa{
    int id, idS;
    String desafio;
    queue desafiosTerminados;
    while true { 
    if
        Competidor[*]?desafioTerminado(id, desafio) do
            push(desafiosTerminados, (id, desafio));
        end desafioTerminado;
    [] when (not empty (desafiosTerminados)) -->
        Supervisor[*]?pedidoDesafio(idS) do
            pop(desafiosTerminados, (id, desafio));
            Supervisor[idS]!desafio(id, desafio);
        end Supervidor;
    }
}

Process Supervisor[id: 1..S]{
    int idC;
    String desafio;
    boolean correcto;
    while true {
        Mesa!pedidoDesafio(id);
        Mesa?desafio(idC, desafio);
        corregir(desafio, correcto);
        Competidor[id]!correccion(correcto);
    }
}