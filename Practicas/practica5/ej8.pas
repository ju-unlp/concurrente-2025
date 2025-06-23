{
Hay un sistema de reconocimiento de huellas dactilares de la policía que tiene 8 Servidores para realizar el reconocimiento, cada uno de ellos trabajando con una Base de Datos propia; a su vez hay un Especialista que utiliza indefinidamente. El sistema funciona de la siguiente manera: el Especialista toma una imagen de una huella (TEST) y se la envía a los servidores para que cada uno de ellos le devuelva el código y el valor de similitud de la huella que más se asemeja a TEST en su BD; al final del procesamiento, el especialista debe conocer el código de la huella con mayor valor de similitud entre las devueltas por los 8 servidores. Cuando ha terminado de procesar una huella comienza nuevamente todo el ciclo. 
    Nota: suponga que existe una función Buscar(test, código, valor) que utiliza cada Servidor donde recibe como parámetro de entrada la huella test, y devuelve como parámetros de salida el código y el valor de similitud de la huella más parecida a test en la BD correspondiente. 
    Maximizar la concurrencia y no generar demora innecesaria.
}

Procedure ej6 is
{ especificación tasks }
    Task Especialista is
        entry PedirHuella(test: out huella);
        entry Evaluacion(codigo: in integer; similitud: in integer);
    End Especialista;
    Task Type Servidores;
{ declaración variables task type }
    arrServidores: array (1..8) of Servidores;
{ cuerpo tasks }
    Task Body Especialista is
        test: huella;
        codigoSimilar, max, pedidos: integer;
    begin
        loop
            pedidos := 0;
            max := -1;
            test := traerHuella(); // asumir que existe
            for (i in (1..16)) loop
                select
                    when (pedidos < 8) => accept PedirHuella(t) do
                        t := test;
                        pedidos := pedidos + 1;
                    end PedirHuella;
                or
                    accept Evaluacion(codigo, similitud) do
                        if(similitud > max) then
                            max := similitud;
                            codigoSimilar := codigo;
                        end if;
                    end Evaluacion;
                end select;
            end loop;
        end loop;
    end Especialista;

    Task Body Servidores is
        test: huella;
        codigo, similitud: integer;
    begin
        loop
            Especialista.pedirHuella(test);
            Buscar(test, codigo, similitud);
            Especialista.Evaluacion(codigo, similitud);
        end loop;
    end Servidores;
begin
    null;
end ej6;