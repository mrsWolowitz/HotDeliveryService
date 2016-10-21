# HotDeliveryService

Тестовое задание: <strong>HotDeliveryService</strong>

Необходимо написать упрощённую версию сервиса по работе со сверхсрочными доставками.

Функциональные компоненты сервиса:

<strong>HTTP API. Методы:</strong>

o <strong>GetAvailableDeliveries()</strong> - возвращает доставки, доступные для взятия (в статусе Available);
o <strong>TakeDelivery(int userId, int deliveryId)</strong> - закрепляет за пользователем доставку (перевод в статус Taken);
<ul type="square">
<li>В случае если доставка не найдена, вернуть 404 с соответствующим сообщением в ответе;</li>
<li>В случае если статус отличается от Available, вернуть 422 с соответствующим сообщением в ответе;</li>
</ul>

<strong>Scheduler. Таски:</strong>

o <strong>CreateDeliveries</strong> - создаёт новые доставки;
<ul type="square">
<li>На каждое срабатывание таски создаётся от N до M (значения хранятся в конфиге) доставок;</li>
<li>Таска срабатывает по расписанию, заданному интервалом в конфиге в секундах (например, при интервале от 10 до 20 секунд - таска в 
первый раз может сработать через 13 секунд после запуска приложения, во второй через 17 и т.д.);</li>
<li>Созданные доставки находятся в статусе Available;</li>
<li>У созданных доставок проставляется ExpirationTime (время жизни доставки в секундах хранится в конфиге);</li>
<li>У созданных доставок проставлять поле Title случайным значением;</li>
</ul>
o <strong>ExpireDeliveries</strong> - находит доставки, ExpirationTime которых находится в прошлом, а статус всё ещё Available и проставляет им статус Expired;

<strong>Требования:</strong>
<ul type="circle">
<li>Приложение должно работать &quot;из коробки&quot;, т.е. не должно требовать установки никакого дополнительного софта (только стандартные библиотеки .NET Framework + Nuget пакеты, исключение - IIS);</li>
<li>Для хранения данных должно быть реализовано два варианта: sqlite и файл произвольного формата (например, xml). То, какой из них должен быть использован, задаётся в конфиге.</li>
<li>HTTP API должно быть разработано в соответствии с REST;</li>
<li>Если произошла непредвиденная ошибка при вызове какого-либо метода, не должно возвращаться никакого стэктрэйса - возвращаем 500-ю ошибку с сообщением InternalServerError;</li>
<li>Поля объекта доставки: Id, Status, Title, UserId, CreationTime, ModificationTime;</li>
