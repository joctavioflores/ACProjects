@echo off
echo: Inicio ciclo de Impresion Directa, no cerrar.
echo:	%1 %2 Esperando instrucciones ...
:inicio
for %%f in (\\%1\Texto\BAT_%2*.bat) do (
	echo:Procesando %%f
	call %%f
	rem ping 1.1.1.1 -n 1 -w 2000> nul
	del %%f
	echo:Ok
)
ping 1.1.1.1 -n 1 -w 2000> nul
goto inicio

