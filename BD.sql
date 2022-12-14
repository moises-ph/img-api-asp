drop database LoginImg

create database LoginImg

--use MotorStore

use LoginImg

create table user_data(
	id_usr varchar(10) primary key,
	contraseņa varchar(max),
	Nombre varchar(30) not null,
	Apellido varchar(30) not null,
	Img_perfil varchar(max)
);


go
create procedure create_usr
	@id_usr varchar(10),
	@contraseņa varchar(max),
	@Nombre varchar(30),
	@Apellido varchar(30)
as
begin transaction tx_Createusr
	BEGIN TRY
		INSERT into user_data(id_usr, contraseņa,Nombre,Apellido, Img_perfil) values (@id_usr,@contraseņa ,@Nombre, @Apellido, 'defualt.png')
		COMMIT transaction tx_Createusr
		Select 'Creado correctamente' as Respuesta, 0 as Error
	END TRY
	BEGIN CATCH
		ROLLBACK transaction tx_Createusr
		SELECT ERROR_MESSAGE() as Respuesta, 1 as Error
	END CATCH


	--SELECT * from user_data

go
create procedure actualizar_perfil
	@id_usr varchar(10),
	@Img_perfil varchar(max)
as
begin transaction tx_ActualizarPerfil
	BEGIN TRY
		UPDATE user_data set Img_perfil = @Img_perfil where id_usr = @id_usr
		COMMIT transaction tx_ActualizarPerfil
		Select 'Foto subida correctamente' as Respuesta, 0 as Error
	END TRY
	BEGIN CATCH
		ROLLBACK transaction tx_ActualizarPerfil
		SELECT ERROR_MESSAGE() as Respuesta, 1 as Error
	END CATCH

execute actualizar_perfil '1235' , 'Gian.jpg'

go
create procedure select_perfil
	@id_usr varchar(10)
as
begin 
SELECT Img_perfil from user_data where id_usr = @id_usr
end

go
create procedure eliminar_perfil
	@id_usr varchar(10)
as
begin transaction Tx_DropProfile
	BEGIN TRY
		UPDATE user_data set Img_perfil = 'defualt.png' where id_usr = @id_usr
		COMMIT Transaction Tx_DropProfile
		Select 'Perfil eliminado correctamente' as Respuesta, 0 as Error	
	END TRY
	BEGIN CATCH
		ROLLBACK transaction Tx_DropProfile
		Select ERROR_MESSAGE() as Respuesta, 1 as Error
	END CATCH
	

go
create procedure actualizar_usr
	@id_usr varchar(10),
	@contraseņa varchar(max),
	@Nombre varchar(30),
	@Apellido varchar(30)
as
begin transaction tx_Updateusr
	BEGIN TRY
		if(exists(select * from user_data where id_usr = @id_usr))
		begin
		UPDATE user_data set contraseņa =  @contraseņa, Nombre = @Nombre, Apellido = @Apellido where id_usr = @id_usr
		COMMIT transaction tx_Updateusr
		Select 'Actualizado Correctamente' as Respuesta, 0 as Error
		end
		else
		begin
			ROLLBACK transaction tx_Updateusr
			SELECT 'Usuario inexistente' as Respuesta, 1 as Error
		end
	END TRY
	BEGIN CATCH
		ROLLBACK transaction tx_Updateusr
		SELECT ERROR_MESSAGE() as Respuesta, 1 as Error
	END CATCH

	

go
create procedure eliminar_usr
	@id_usr varchar(10)
as
begin transaction tx_Deleteusr
	BEGIN TRY
		if(exists(select * from user_data where id_usr = @id_usr))
		begin
			delete from user_data where id_usr = @id_usr
			COMMIT transaction tx_Deleteusr
			Select 'Eliminado Correctamente' as Respuesta, 0 as Error
		end
		else
		begin
			ROLLBACK transaction tx_Deleteusr
			SELECT 'Usuario inexistente' as Respuesta, 1 as Error
		end
	END TRY
	BEGIN CATCH
		ROLLBACK transaction tx_Deleteusr
		SELECT ERROR_MESSAGE() as Respuesta, 1 as Error
	END CATCH

	--execute eliminar_usr '1234'




go
create procedure select_usr
	@id_usr varchar(10)
as
begin
	SELECT Nombre, Apellido FROM user_data where id_usr = @id_usr
end
