{/*
Se quiere modelar el funcionamiento de un banco, al cual llegan clientes que deben realizar un pago y retirar un comprobante. Existe un único empleado en el banco, el cual atiende de acuerdo con el orden de llegada.
    
    b. Implemente una solución donde los clientes se retiran si esperan más de 10 minutos para realizar el pago.
*/}

// banco 
// clientes hacen pago y sacan comprobante
// empleado atiende con orden de llegada

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
        Select
            Empleado.Atencion(pago, c);
        or Delay 600
            null
        End select;
    End Clientes;
    //
    Task Body Empleado is 
        pago: integer;
        c: texto;
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