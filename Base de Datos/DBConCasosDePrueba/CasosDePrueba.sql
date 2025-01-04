INSERT INTO master.dbo.Users (Email,Password,Name,Lastname,AccountCreation,ProfilePicturePath) VALUES
	 (N'co1@smarthome.com',N'sA$a1234',N'alejo',N'fraga','2024-11-18 12:54:50.1484010',NULL),
	 (N'co2@smarthome.com',N'sA$a1234',N'sebastian',N'vega','2024-11-18 12:55:01.5528933',NULL),
	 (N'co3@smarthome.com',N'sA$a1234',N'matias',N'corvetto','2024-11-18 12:55:42.1331156',NULL),
	 (N'ho1@smarthome.com',N'sA$a1234',N'juan',N'fernandez','2024-11-18 12:52:29.4466839',N'https://images.imagenmia.com/model_version/bbfea91410ef7994cfefde4a33e032f3aebf7b90dda683f7fa32ea2685d2e7bb/1723819204347-output.jpg'),
	 (N'ho2@smarthome.com',N'sA$a1234',N'carla',N'gutierrez','2024-11-18 12:53:06.8576210',N'https://b2472105.smushcdn.com/2472105/wp-content/uploads/2023/09/Poses-Perfil-Profesional-Mujeres-ago.-10-2023-1-819x1024.jpg?lossy=1&strip=1&webp=1'),
	 (N'ho3@smarthome.com',N'sA$a1234',N'jhon',N'smith','2024-11-18 12:54:13.1143869',N'https://vivolabs.es/wp-content/uploads/2022/03/perfil-hombre-vivo.png'),
	 (N'sa2@smarthome.com',N'sA$a1234',N'lionel',N'pereira','2024-11-18 12:56:24.1674452',NULL),
	 (N'sa3@smarthome.com',N'sA$a1234',N'luis',N'morales','2024-11-18 12:56:42.1515341',NULL);

INSERT INTO master.dbo.UserRole (RoleName,UserEmail) VALUES
	 (N'CompanyOwner',N'co1@smarthome.com'),
	 (N'CompanyOwner',N'co2@smarthome.com'),
	 (N'CompanyOwner',N'co3@smarthome.com'),
	 (N'HomeOwner',N'ho1@smarthome.com'),
	 (N'HomeOwner',N'ho2@smarthome.com'),
	 (N'HomeOwner',N'ho3@smarthome.com'),
	 (N'Admin',N'sa2@smarthome.com'),
	 (N'Admin',N'sa3@smarthome.com');

INSERT INTO master.dbo.Companies (RUT,Name,LogoUrl,OwnerEmail,ValidatorTypeName) VALUES
	 (N'123456789123',N'Empresa de Alejo',N'https://static.vecteezy.com/system/resources/previews/005/040/068/non_2x/af-a-f-letter-logo-with-colorblock-design-and-creative-cut-vector.jpg',N'co1@smarthome.com',N'ValidatorLettersAndNumbers'),
	 (N'145785412365',N'Empresa de Seba',N'https://st4.depositphotos.com/4263287/24537/v/450/depositphotos_245371396-stock-illustration-combination-letter-alphabet-logo-icon.jpg',N'co2@smarthome.com',N'ValidatorLettersAndNumbers'),
	 (N'523654125478',N'Empresa de Matias',N'https://static.vecteezy.com/system/resources/previews/012/731/827/non_2x/mc-initial-letter-gold-calligraphic-feminine-floral-hand-drawn-heraldic-monogram-antique-vintage-style-luxury-logo-design-premium-vector.jpg',N'co3@smarthome.com',N'ValidatorLength');


INSERT INTO master.dbo.Devices (ModelNumber,Name,Description,Photos,CompanyRUT,HasMovementDetection,HasPersonDetection,IsOutdoor,IsIndoor,DeviceType,DeviceTypeName) VALUES
	 (N'111AAA',N'Sensor de puerta para hogar generico',N'Su montaje es simple ya que solo requiere que se adhiera al marco de la puerta o ventana Previamente debe asociarse a un panel de alarma que sera el encargado de notificar si el sensor es activado para brindarte la total tranquilidad de que seras notificado en caso de que sea necesario Los sensores son inalambricos funcionan con pila de reloj Su durabilidad va a depender de la cantidad de veces que se activen los sensores pero habitualmente es de meses',N'["https://www.hlc.com.uy/imgs/productos/productos3_7297.png"]',N'123456789123',NULL,NULL,NULL,NULL,N'Device',N'Sensor'),
	 (N'252YTG',N'Sensor de Movimiento marca XION',N'Este sensor de movimiento es genial y sirve para detectar intrusos en tu hogar',N'["https://cdn.prod.website-files.com/62a811481c9610ad2f6310b2/653190a2274fadce29dcf234_C%C3%B3mo%20funciona%20un%20sensor%20de%20movimiento.jpg"]',N'123456789123',NULL,NULL,NULL,NULL,N'Device',N'MovementSensor'),
	 (N'454KKK',N'Lampara marca TATA',N'Esta lampara es de mediana calidad Tiene Garantia de seis meses',N'["https://f.fcdn.app/imgs/674102/www.lasa.com.uy/lasauy/d6a9/original/catalogo/maderaymetal_maderaymetal_1/2000-2000/lampara-articulada-de-escritorio-en-madera-y-metal-lampara-articulada-de-escritorio-en-madera-y-metal.jpg"]',N'123456789123',NULL,NULL,NULL,NULL,N'Device',N'Lamp'),
	 (N'777HGF',N'Camara para dentro de hogar marca RUK',N'Esta camara funciona dentro de hogares y sirve para detetar movimiento y personas',N'["https://www.circuit.com.uy/images/thumbs/0102487_camara-de-vigilancia-tp-link-vigi-c430-wifi_415.jpeg"]',N'123456789123',1,1,0,1,N'Camera',N'Camera');


INSERT INTO master.dbo.Homes (Id,OwnerEmail,MemberCount,Name) VALUES
	 (N'CFE07B61-7A71-419D-B2CC-625FFCAE1065',N'ho1@smarthome.com',3,N'casa de juan'),
	 (N'4BF1526A-C547-4AF5-8DE3-94F517D89ECE',N'ho2@smarthome.com',2,N'casa de carla');

	
INSERT INTO master.dbo.Coordinates (Latitude,Longitude,HomeId) VALUES
	 (N'10',N'-50',N'CFE07B61-7A71-419D-B2CC-625FFCAE1065'),
	 (N'45',N'39',N'4BF1526A-C547-4AF5-8DE3-94F517D89ECE');


INSERT INTO master.dbo.Locations (Address,DoorNumber,HomeId) VALUES
	 (N'silver street',N'800',N'CFE07B61-7A71-419D-B2CC-625FFCAE1065'),
	 (N'calle dorada',N'333',N'4BF1526A-C547-4AF5-8DE3-94F517D89ECE');

	
INSERT INTO master.dbo.Rooms (Id,Name,HomeId) VALUES
	 (N'A72F0C26-2D41-40F7-87BC-8EEC7EF0D1AC',N'hall',N'CFE07B61-7A71-419D-B2CC-625FFCAE1065'),
	 (N'9118860A-C3A6-40CC-AE28-FFD68EA3E5D0',N'habitacion secreta',N'CFE07B61-7A71-419D-B2CC-625FFCAE1065');


INSERT INTO master.dbo.Hardwares (Id,DeviceModelNumber,Connected,HomeId,Discriminator,IsOn,Name,RoomId,IsOpen) VALUES
	 (N'62BF3427-100F-4994-B9A0-0737D49E4825',N'252YTG',1,N'CFE07B61-7A71-419D-B2CC-625FFCAE1065',N'Hardware',NULL,N'Sensor de Movimiento marca XION',N'A72F0C26-2D41-40F7-87BC-8EEC7EF0D1AC',NULL),
	 (N'77041996-69A5-48AC-BE75-26A84DF323F9',N'454KKK',1,N'CFE07B61-7A71-419D-B2CC-625FFCAE1065',N'LampHardware',0,N'Lampara marca TATA',N'A72F0C26-2D41-40F7-87BC-8EEC7EF0D1AC',NULL),
	 (N'2E901D4F-5881-462D-8213-3AF81B2CA3E8',N'777HGF',1,N'CFE07B61-7A71-419D-B2CC-625FFCAE1065',N'Hardware',NULL,N'Caja fuerte',N'9118860A-C3A6-40CC-AE28-FFD68EA3E5D0',NULL),
	 (N'4E54D2B4-BA9E-4B52-A01C-6E86691D1F6C',N'777HGF',1,N'CFE07B61-7A71-419D-B2CC-625FFCAE1065',N'Hardware',NULL,N'pasillo',N'9118860A-C3A6-40CC-AE28-FFD68EA3E5D0',NULL),
	 (N'3CBDD425-AED0-41CC-86D4-7EDB2634F6C2',N'454KKK',1,N'CFE07B61-7A71-419D-B2CC-625FFCAE1065',N'LampHardware',0,N'Lampara marca TATA',NULL,NULL),
	 (N'9A1959A4-FF13-4570-AA93-90DD6653249E',N'777HGF',1,N'CFE07B61-7A71-419D-B2CC-625FFCAE1065',N'Hardware',NULL,N'Camara para dentro de hogar marca RUK',N'9118860A-C3A6-40CC-AE28-FFD68EA3E5D0',NULL),
	 (N'ED1EB940-7183-4048-A9EF-AB1909089E90',N'111AAA',1,N'CFE07B61-7A71-419D-B2CC-625FFCAE1065',N'SensorHardware',NULL,N'Sensor de puerta para hogar generico',N'A72F0C26-2D41-40F7-87BC-8EEC7EF0D1AC',1),
	 (N'31A9D93F-80CD-45A5-A4FD-C1252F8A9AAD',N'111AAA',1,N'CFE07B61-7A71-419D-B2CC-625FFCAE1065',N'SensorHardware',NULL,N'Sensor de puerta para hogar generico',NULL,0);


INSERT INTO master.dbo.Members (Id,UserEmail,HomeId) VALUES
	 (N'968B5353-856A-483A-A97E-15434B0BFE5C',N'ho3@smarthome.com',N'4BF1526A-C547-4AF5-8DE3-94F517D89ECE'),
	 (N'386550EF-2E86-4747-B077-66C275A5CC6A',N'ho1@smarthome.com',N'CFE07B61-7A71-419D-B2CC-625FFCAE1065'),
	 (N'DCAE2B24-5627-4BD8-948E-DB50F7FA90A1',N'ho2@smarthome.com',N'CFE07B61-7A71-419D-B2CC-625FFCAE1065'),
	 (N'F06959E5-37EB-49BC-9347-E8C2FF40E72A',N'ho2@smarthome.com',N'4BF1526A-C547-4AF5-8DE3-94F517D89ECE');



INSERT INTO master.dbo.HomePermissions (Name,MemberId) VALUES
	 (N'AddDevice',N'968B5353-856A-483A-A97E-15434B0BFE5C'),
	 (N'AddDeviceToRoom',N'968B5353-856A-483A-A97E-15434B0BFE5C'),
	 (N'ChangeDeviceName',N'968B5353-856A-483A-A97E-15434B0BFE5C'),
	 (N'ChangeHomeName',N'968B5353-856A-483A-A97E-15434B0BFE5C'),
	 (N'GrantHomePermissions',N'968B5353-856A-483A-A97E-15434B0BFE5C'),
	 (N'ListDevices',N'968B5353-856A-483A-A97E-15434B0BFE5C'),
	 (N'ReceiveNotifications',N'968B5353-856A-483A-A97E-15434B0BFE5C'),
	 (N'AddDevice',N'386550EF-2E86-4747-B077-66C275A5CC6A'),
	 (N'AddDeviceToRoom',N'386550EF-2E86-4747-B077-66C275A5CC6A'),
	 (N'ChangeDeviceName',N'386550EF-2E86-4747-B077-66C275A5CC6A');

INSERT INTO master.dbo.HomePermissions (Name,MemberId) VALUES
	 (N'ChangeHomeName',N'386550EF-2E86-4747-B077-66C275A5CC6A'),
	 (N'CreateRoom',N'386550EF-2E86-4747-B077-66C275A5CC6A'),
	 (N'GrantHomePermissions',N'386550EF-2E86-4747-B077-66C275A5CC6A'),
	 (N'ListDevices',N'386550EF-2E86-4747-B077-66C275A5CC6A'),
	 (N'ReceiveNotifications',N'386550EF-2E86-4747-B077-66C275A5CC6A'),
	 (N'AddDevice',N'DCAE2B24-5627-4BD8-948E-DB50F7FA90A1'),
	 (N'ListDevices',N'DCAE2B24-5627-4BD8-948E-DB50F7FA90A1'),
	 (N'AddDevice',N'F06959E5-37EB-49BC-9347-E8C2FF40E72A'),
	 (N'AddDeviceToRoom',N'F06959E5-37EB-49BC-9347-E8C2FF40E72A'),
	 (N'ChangeDeviceName',N'F06959E5-37EB-49BC-9347-E8C2FF40E72A');
INSERT INTO master.dbo.HomePermissions (Name,MemberId) VALUES
	 (N'ChangeHomeName',N'F06959E5-37EB-49BC-9347-E8C2FF40E72A'),
	 (N'CreateRoom',N'F06959E5-37EB-49BC-9347-E8C2FF40E72A'),
	 (N'GrantHomePermissions',N'F06959E5-37EB-49BC-9347-E8C2FF40E72A'),
	 (N'ListDevices',N'F06959E5-37EB-49BC-9347-E8C2FF40E72A'),
	 (N'ReceiveNotifications',N'F06959E5-37EB-49BC-9347-E8C2FF40E72A');


INSERT INTO master.dbo.Notifications (Id,Message,[Date],HardwareId) VALUES
	 (N'EAC7A228-9FE1-447C-AB38-6D9DC54321A5',N'Movement detected!','2024-11-18 13:31:00.0435442',N'2E901D4F-5881-462D-8213-3AF81B2CA3E8'),
	 (N'42584702-5577-4921-A485-BC2244582C1B',N'Person detected! User identified: carla gutierrez - ho2@smarthome.com','2024-11-18 13:31:20.2237284',N'2E901D4F-5881-462D-8213-3AF81B2CA3E8'),
	 (N'4DED9148-6D9B-4A34-8D1F-E13940D0635C',N'Window movement: opened','2024-11-18 13:23:04.7300292',N'ED1EB940-7183-4048-A9EF-AB1909089E90');


INSERT INTO master.dbo.NotiActions (NotificationId,MemberId,IsRead,HomeId) VALUES
	 (N'EAC7A228-9FE1-447C-AB38-6D9DC54321A5',N'386550EF-2E86-4747-B077-66C275A5CC6A',0,N'CFE07B61-7A71-419D-B2CC-625FFCAE1065'),
	 (N'42584702-5577-4921-A485-BC2244582C1B',N'386550EF-2E86-4747-B077-66C275A5CC6A',0,N'CFE07B61-7A71-419D-B2CC-625FFCAE1065'),
	 (N'4DED9148-6D9B-4A34-8D1F-E13940D0635C',N'386550EF-2E86-4747-B077-66C275A5CC6A',1,N'CFE07B61-7A71-419D-B2CC-625FFCAE1065');
