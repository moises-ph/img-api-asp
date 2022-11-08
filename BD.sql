create database LoginImg

use LoginImg

create table auth_User(
	id_usr varchar(10) primary key,
	contraseña varchar(max) not null,
);

create table data_user(
	id_usr varchar(10),
	Nombre varchar(30) not null,
	Apellido varchar(30) not null,
	Img_perfil varbinary(max),
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
		INSERT into auth_User(id_usr, contraseña) values (@id_usr, @contraseña)
		INSERT into data_user(id_usr,Nombre,Apellido) values (@id_usr, @Nombre, @Apellido)
		COMMIT transaction tx_Createusr
		Select 'Creado correctamente' as Respuesta, 0 as Error
	END TRY
	BEGIN CATCH
		ROLLBACK transaction tx_Createusr
		SELECT ERROR_MESSAGE() as Respuesta, 1 as Error
	END CATCH

execute create_usr '1234567890', 'xd', 'Moises', 'Pineda'

go
create procedure profile_usr
	@id_usr varchar(10),
	@img varbinary(max)
as
begin transaction tx_ProfileUsr
	BEGIN TRY
		IF(exists(select * from data_user where id_usr = @id_usr))
		begin
			UPDATE data_user set Img_perfil = @img
			SELECT 'imagen actualizada correctamente' as Respuesta, 0 as Error
			COMMIT transaction tx_ProfileUsr
		end
		else
		BEGIN
			SELECT 'Usuario no existe' as Respuesta, 1 as Error
		END
	END TRY
	BEGIN CATCH
		ROLLBACK transaction tx_ProfileUsr
		SELECT ERROR_MESSAGE() as Respuesta, 1 as Error
	END CATCH



drop procedure create_usr

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
		if(exists(select * from auth_User where id_usr = @id_usr))
		begin
		UPDATE auth_User set contraseña = @contraseña where id_usr = @id_usr
		UPDATE data_user set Nombre = @Nombre, Apellido = @Apellido, Img_perfil = @Img_perfil where id_usr = @id_usr
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
		if(exists(select * from auth_User where id_usr = @id_usr))
		begin
			delete from data_user where id_usr = @id_usr
			delete from auth_User where id_usr = @id_usr
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


go
create procedure select_img_usr
	@id_usr varchar(10)
as
begin
	SELECT Img_perfil from data_user where id_usr = @id_usr
end
