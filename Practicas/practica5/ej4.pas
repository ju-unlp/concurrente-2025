
{/*
Se debe calcular el valor promedio de un vector de 1 millón de números enteros que se encuentra distribuido entre 10 procesos Worker (es decir, cada Worker tiene un vector de 100 mil números). Para ello, existe un Coordinador que determina el momento en que se debe realizar el cálculo de este promedio y que, además, se queda con el resultado. 
    Nota: maximizar la concurrencia; este cálculo se hace una sola vez.
*/}

Procedure Promedio is
    //* especificación de las tasks */
    Task Coordinador is 
        Entry Listo(id: in integer);
        Entry Sumar(nro: int integer);
    End Coordinador;
    Task Type Worker is
        Identificar(id: in integer);
        Contar();
    End Worker;
    //* declaración de variables typetask */
    arrWorkers: array (1..10) of Worker;
    //* cuerpo de las tasks */
    Task Body Coordinador is
        total, promedio: integer;
    Begin
        for (i in 1..20) loop
            select
                accept Listo(id: in integer) do
                    arrWorkers[id].Contar();
                end Listo;
            or
                accept Sumar(nro: in integer) do
                    total := total + nro;
                end Sumar;
            end select;
        end loop;
        promedio := total / 1000000;
    End Coordinador;

    Task Body Worker is
        id, total: integer;
        vec: array 1..100000 of integer;
    Begin
        accept Identificar(pos) do 
            id := pos;
        end Identificar;

        Coordinador.Listo(id);
        accept Contar();
        total := sumar(vec); // asumir que existe
        Coordinador.Sumar(total);
    End Worker;
Begin
    for (i in 1..10) loop
        arrWorkers[i].Identificar(i);
    end loop;
End Promedio;
