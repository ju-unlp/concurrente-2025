Necesito asistencia para corregir una práctica de programación concurrente. La idea es que te voy a pasar consignas y resoluciones o preguntas acerca de eso y tenes que ayudarme a guiarme a la solución correcta a partir de los parámetros. A menos que te pida la solución correcta o corregida, necesito que solo me marques los lugares en los que hay errores y una guia de cual es el error. Tus respuestas deben ser claras y consisas, aptas para un ambiente universitario. Estoy trabajando en ADA. Especial foco en mantener las respuestas consisas, claras y profesionales, actua como tutor universitario.

Actualmente estoy trabajando sobre rendezvous. las precondiciones son:

1. NO SE PUEDE USAR VARIABLES COMPARTIDAS
2. Declaración de tareas
• Especificación de tareas sin ENTRY’s (nadie le puede hacer llamados).
TASK Nombre;
TASK TYPE Nombre;
• Especificación de tareas con ENTRY’s (le puede hacer llamados). Los entry’s
funcionan de manera semejante los procedimientos: solo pueden recibir o
enviar información por medio de los parámetros del entry. NO RETORNAN
VALORES COMO LAS FUNCIONES
TASK [TYPE] Nombre IS
ENTRY e1;
ENTRY e2 (p1: IN integer; p2: OUT char; p3: IN OUT float);
END Nombre;
• Cuerpo de las tareas.
TASK BODY Nombre IS
Codigo que realiza la Tarea;
END Nombre;
3. Sincronización y comunicación entre tareas
• Entry call para enviar información (o avisar algún evento).
NombreTarea.NombreEntry (parametros);
• Accept para atender un pedido de entry call sin cuerpo (sólo para recibir el
aviso de un evento para sincronización). Lo usual es que no incluya
parámetros, aunque podría tenerlos. En ese caso, son ignorados por no tener
cuerpo presente.
ACCEPT NombreEntry;
ACCEPT NombreEntry (p1: IN integer; p3: IN OUT float);
• Accept para atender un pedido de entry call con cuerpo.
ACCEPT NombreEntry (p1: IN integer; p3: IN OUT float) do
Cuerpo del accept donde se puede acceder a los parámetros p1 y p3.
Fuera del entry estos parámetros no se pueden usar.
END NombreEntry;
• El accept se puede hacer en el cuerpo de la tarea que ha declarado el entry en
su especificación. Los entry call se pueden hacer en cualquier tarea o en el
programa principal.
4. Select para ENTRY CALL.
• Select ...OR DELAY: espera a lo sumo x tiempo a que la tarea correspondiente haga el
accept del entry call realizado. Si pasó el tiempo entonces realiza el código opcional.
SELECT
NombreTarea.NombreEntry(Parametros);
Sentencias;
OR DELAY x
Código opcional;
END SELECT;
• Select ...ELSE: si la tarea correspondiente no puede realizar el accept inmediatamente
(en el momento que el procesador está ejecutando esa línea de código) entonces se
ejecuta el código opcional.
SELECT
NombreTarea.NombreEntry(Parametros);
Sentencias;
ELSE
Código opcional;
END SELECT;
• En los select para entry call sólo puede ponerse un entry call y una única opción (OR
DELAY o ELSE);
5. Select para ACCEPT.
• En los select para los accept puede haber más de una alternativa de accept, pero no
puede haber alternativas de entry call (no se puede mezclar accept con entries). Cada
alternativa de ACCEPT puede ser o no condicional (uso de cláusula WHEN).
SELECT
ACCEPT e1 (parámetros);
Sentencias1;
OR
ACCEPT e2 (parámetros) IS cuerpo; END e2;
OR
WHEN (condición) => ACCEPT e3 (parámetros) IS cuerpo; END e3;
Sentencias3
END SELECT;
Funcionamiento: Se evalúa la condición booleana del WHEN de cada alternativa (si
no lo tiene se considera TRUE). Si todas son FALSAS se sale del select. En otro caso,
de las alternativas cuya condición es verdadera se elige en forma no determinística una
que pueda ejecutarse inmediatamente (es decir que tiene un entry call pendiente). Si
ninguna de ellas se puede ejecutar inmediatamente el select se bloquea hasta que haya
un entry call para alguna alternativa cuya condición sea TRUE.
• Se puede poner una opción OR DELAY o ELSE (no las dos a la vez).
• Dentro de la condición booleana de una alternativa (en el WHEN) se puede preguntar
por la cantidad de entry call pendientes de cualquier entry de la tarea.
NombreEntry’count
• Después de escribir una condición por medio de un WHEN siempre se debe escribir un
accept.


EJEMPLO DE CONSIGNA Y RESULTADO PARA USAR DE REFERENCIA:
ejemplo 1:
Se debe modelar la atención en un banco por medio de un dos empleados. Los
clientes llegan y son atendidos de acuerdo al orden de llegada.

```ada
Procedure Banco4 is
Task Administrador is
entry Pedido (IdC: IN integer; D: IN texto);
entry Siguiente (Id: OUT integer; DC: OUT texto);
End Administrador;
Task Type Cliente is
entry Ident (Pos: IN integer);
entry Respuesta (R: IN texto);
End Cliente;
Task Type Empleado;
EmpleadoA, EmpleadoB: Empleado;
arrClientes: array (1..N) of Cliente;
Task Body Administrador is
Begin
loop
Accept Siguiente (Id: OUT integer; DC: OUT texto) do
Accept Pedido (IdC: IN integer; D: IN texto) do
Id := IdC;
DC := D;
End Pedido;
End Siguiente;
end loop;
End Administrador;Task Body Empleado is
Res, Dat: texto; idC: integer;
Begin
loop
Administrador.Siguiente (idC, Dat);
Res := resolverPedido(Dat);
arrClientes(idC).Respuesta(Res);
end loop;
End Empleado;
Task Body cliente is
Resultado: texto; id: integer;
Begin
Accept Ident (Pos: IN integer) do
id := Pos;
end Ident;
Administrador.Pedido (id, “datos”);
Accept Respuesta (R: IN texto) do
Resultado := R;
end Respuesta;
End cliente;
Begin
for i in 1..N loop
arrClientes(i).Ident(i);
end loop;
End Banco4
```

Ejemplo 2:
Se debe modelar un buscador para contar la cantidad de veces que aparece un número dentro de un
vector distribuido entre las
N tareas contador. Además existe un
administrador que decide el
número que se desea buscar y se lo envía a los N contadores para que lo busquen en la parte del
vector que poseen, y calcula la cantidad total.

```ada
Procedure ContadorOcurrencias is
Task Admin is
entry Valor (num: out integer);
entry Resultado (res: in integer);
End admin;
Task type Contador;
ArrC: array (1..N) of Contador;
Task body Contador is
vec: array (1..V) of integer := InicializarVector;
valor, cant: integer :=0;
Begin
Admin.valor(valor);
for i in 1..V loop
if (vec(i) = valor) then
cant:=cant+1;
end if;
end loop;
Admin.Resultado(cant);
End contador
Task body Admin is
numero: integer := elegirNumero; total: integer := 0;
Begin
for i in 1..2*N loop
select
accept Valor (num: out integer) do
num := numero;
end Valor;
or
accept Resultado (res: in integer) do
total:= total + res;
end Resultado;
end select;
end loop;
End Admin;
Begin
null;
End ContadorOcurrencias;
```

responde OK a este mensaje