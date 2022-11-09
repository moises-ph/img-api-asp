create database LoginImg

use LoginImg

create table user_data(
	id_usr varchar(10),
	contraseña varchar(max),
	Nombre varchar(30) not null,
	Apellido varchar(30) not null,
	Img_perfil varchar(max),
	constraint fk_id_usr foreign key (id_usr) references auth_User(id_usr)
);

go
create procedure create_usr
	@id_usr varchar(10),
	@contraseña varchar(max),
	@Nombre varchar(30),
	@Apellido varchar(30)
as
begin transaction tx_Createusr
	BEGIN TRY
		INSERT into user_data(id_usr, contraseña,Nombre,Apellido) values (@id_usr,@contraseña ,@Nombre, @Apellido)
		COMMIT transaction tx_Createusr
		Select 'Creado correctamente' as Respuesta, 0 as Error
	END TRY
	BEGIN CATCH
		ROLLBACK transaction tx_Createusr
		SELECT ERROR_MESSAGE() as Respuesta, 1 as Error
	END CATCH



go
create procedure actualizar_usr
	@id_usr varchar(10),
	@contraseña varchar(max),
	@Nombre varchar(30),
	@Apellido varchar(30),
	@Img_perfil varbinary(max)
as
begin transaction tx_Updateusr
	BEGIN TRY
		if(exists(select * from user_data where id_usr = @id_usr))
		begin
		UPDATE user_data set contraseña =  @contraseña, Nombre = @Nombre, Apellido = @Apellido, Img_perfil = @Img_perfil where id_usr = @id_usr
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

	execute eliminar_usr '1234567891'

	delete from auth_User where id_usr = '12'

	drop procedure eliminar_usr

	SELECT * FROM auth_User, data_user

go
create procedure select_usr
	@id_usr varchar(10)
as
begin
	SELECT Nombre, Apellido FROM data_user where id_usr = @id_usr
end
