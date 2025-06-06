{/*
Se quiere modelar el funcionamiento de un banco, al cual llegan clientes que deben realizar un pago y retirar un comprobante. Existe un único empleado en el banco, el cual atiende de acuerdo con el orden de llegada.

    d. Implemente una solución donde los clientes esperan a lo sumo 10 minutos para ser atendidos. Si pasado ese lapso no fueron atendidos, entonces solicitan atención una vez más y se retiran si no son atendidos inmediatamente.
*/}

Procedure Banco is
    //* especificación tasks */
    Task Empleado is
        Entry Atencion(Pago: IN integer; comprobante: OUT texto);
    End Empleado;
    Task Type Clientes;
    //* declaración variables tasktype */
    arrClientes : array(1..C) of Clientes;
    //* cuerpo tasks */
    Task Body Clientes is
        c: texto;
        pago: integer;
    Begin
        select
            Empleado.Atencion(pago, c);
        or delay 600
            select 
                Empleado.Atencion(pago,c);
            else 
                null
            end select;
        end select;
    End Clientes;
    //
    Task Body Empleado is 
    Begin
        loop 
            Accept Atencion(pago, c) do
                    c := generarComprobante(pago);
                End Atencion;
        end loop;
    End Empleado;
Begin
    null;
End Banco;