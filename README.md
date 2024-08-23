## Api .Net 8.0

### Autenticaci�n y Autorizaci�n
P�ginaci�n
CORS
Delegados

Identity Server 4 (https://identityserver.io/) es una herramienta de c�digo abierto que se utiliza para gestionar la autenticaci�n y la autorizaci�n en aplicaciones web y m�viles. 
    * es una implementaci�n de OpenID Connect y OAuth 2.0, que son protocolos de autenticaci�n y autorizaci�n est�ndar en la industria. 
    * proporciona una soluci�n centralizada para la autenticaci�n y la autorizaci�n, lo que facilita la implementaci�n de la seguridad en las aplicaciones. 

Qu� otras herramientas para autenticaci�n y autorizaci�n existe aparte de Identity Server?

* Auth0 => Auth0 es una plataforma de identidad en la nube que se utiliza para gestionar la autenticaci�n y la autorizaci�n en aplicaciones web y m�viles.
* Okta => Okta es una plataforma de identidad en la nube que se utiliza para gestionar la autenticaci�n y la autorizaci�n en aplicaciones web y m�viles.
* Keycloak es una herramienta de c�digo abierto que se utiliza para gestionar la autenticaci�n y la autorizaci�n en aplicaciones web y m�viles.
* AWS Cognito es un servicio de autenticaci�n y autorizaci�n de AWS que se utiliza para gestionar la autenticaci�n y la autorizaci�n en aplicaciones web y m�viles.
* Firebase Authentication es un servicio de autenticaci�n de Firebase que se utiliza para gestionar la autenticaci�n y la autorizaci�n en aplicaciones web y m�viles.
* OAuth.io es una plataforma de autenticaci�n y autorizaci�n que se utiliza para gestionar la autenticaci�n y la autorizaci�n en aplicaciones web y m�viles.
* Stormpath, Ping Identity, ForgeRock, Gluu, Shibboleth, SimpleSAMLphp

Diferencias entre Identity y Identity Server 4

Identity es una herramienta de Microsoft que se utiliza para gestionar la autenticaci�n y la autorizaci�n en aplicaciones web y m�viles.
Identity Server 4 es una herramienta de c�digo abierto que se utiliza para gestionar la autenticaci�n y la autorizaci�n en aplicaciones web y m�viles.

### Paginaci�n parte II

Para trabajar con HttpContext en .NET Core, primero debemos agregar el paquete Microsoft.AspNetCore.Http a nuestro proyecto.

Vamos a implementar CORS. 
CORS (Cross-Origin Resource Sharing) es un mecanismo que permite a los navegadores realizar solicitudes a un servidor desde un origen diferente al del propio servidor. 
CORS es un mecanismo de seguridad que protege a los usuarios de sitios web maliciosos que intentan robar informaci�n de los usuarios.

### B�squeda con predicados

Un delegado es un tipo de referencia que se utiliza para almacenar una referencia a un m�todo.
En c# hay varios tipos de delegados:

* Action: representa un m�todo que no devuelve un valor.
* Func: representa un m�todo que devuelve un valor.
* Predicate: representa un m�todo que devuelve un valor booleano.

Delegados en C# son similares a los punteros a funciones en C y C++, pero son m�s seguros y f�ciles de usar.
Ejemplo de delegado en C#:
```csharp
using System;

````

Result Pattern es un patr�n de dise�o que se utiliza para representar el resultado de una operaci�n que puede tener �xito o fallar.
Es una forma de manejar los errores de una manera m�s segura y eficiente que las excepciones.
Es una alternativa a las excepciones que se utiliza en algunos lenguajes de programaci�n, como Rust y F#.

Ejemplo de Result Pattern en C#:
```csharp
using System;


````

### Autenticaci�n y Autorizaci�n

Identity es una herramienta de Microsoft que se utiliza para gestionar la autenticaci�n y la autorizaci�n en aplicaciones web y m�viles.
Vamos a instalar Identity en nuestro proyecto de .NET Core.

Para instalar Identity en nuestro proyecto de .NET Core, 
primero debemos agregar el paquete Microsoft.AspNetCore.Identity.EntityFrameworkCore a nuestro proyecto.
Y luego el paquete Microsoft.AspNetCore.Authentication.JwtBear a nuestro proyecto.
Microsoft.AspNetCore.Authentication.JwtBear es un paquete que se utiliza para implementar la autenticaci�n basada en tokens JWT en aplicaciones web y m�viles.


### Migraciones autom�ticas y seed data
Seed Data es un conjunto de datos que se utiliza para inicializar una base de datos con datos de prueba.

### Register y Login (uso de options pattern)


### Reset y change password feature

### Refactorizando Sales (con el uso de los claims)

### Probando con swagger y postmam

### Trabajando con archivos y validaciones personalizadas

### Guardar archivos en la nube con Azure Storage y en local

Azure Storage es un servicio de almacenamiento en la nube de Microsoft 
que se utiliza para almacenar datos de forma segura y escalable.

### Minimal APIs, Healthcheck y Excepciones globales

### Preparando proyecto para subir a producci�n y UnitTest

### Subiendo proyecto a la nube de Azure: AppService, SQL Server, SQL Database
App Service es un servicio de Azure que se utiliza para implementar aplicaciones web y m�viles en la nube.
