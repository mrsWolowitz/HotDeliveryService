# HotDeliveryService

HTTP API - HotDeliveryHttp. 
Настройки в web.config. В секции appSettings DBFormat задает тип хранилища данных. В секции connectionStrings путь к соответствующему хранилищу данных.

Scheduler - ShedulerTasks. 
Настройки передаются из клиента. Клиент для Scheduler - ClientTasks с настройками в App.config. В секции appSettings DBFormat задает тип хранилища данных и значения параметров тасков. В секции connectionStrings путь к соответствующему хранилищу данных.

При запуске HotDeliveryHttp бывает возникает ошибка. Нужно переложить из HotDeliveryHttp\bin\x86 библиотеку sqlite3.dll в папку HotDeliveryHttp\bin. Буду благодарна за подсказку как сделать это автоматически.
