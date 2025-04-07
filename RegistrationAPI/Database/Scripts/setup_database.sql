
CREATE DATABASE RegistrationDB;


\c RegistrationDB;


CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,
    fullname VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    phone VARCHAR(20) NOT NULL,
    password VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);


CREATE OR REPLACE FUNCTION sp_create_user(
    p_fullname VARCHAR,
    p_email VARCHAR,
    p_phone VARCHAR,
    p_password VARCHAR
)
RETURNS TABLE (
    id INTEGER,
    fullname VARCHAR,
    email VARCHAR,
    phone VARCHAR,
    created_at TIMESTAMP
) AS $$
BEGIN
    INSERT INTO users (fullname, email, phone, password)
    VALUES (p_fullname, p_email, p_phone, p_password)
    RETURNING users.id, users.fullname, users.email, users.phone, users.created_at;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sp_get_all_users()
RETURNS TABLE (
    id INTEGER,
    fullname VARCHAR,
    email VARCHAR,
    phone VARCHAR,
    created_at TIMESTAMP
) AS $$
BEGIN
    RETURN QUERY
    SELECT u.id, u.fullname, u.email, u.phone, u.created_at
    FROM users u;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sp_get_user_by_id(p_id INTEGER)
RETURNS TABLE (
    id INTEGER,
    fullname VARCHAR,
    email VARCHAR,
    phone VARCHAR,
    created_at TIMESTAMP
) AS $$
BEGIN
    RETURN QUERY
    SELECT u.id, u.fullname, u.email, u.phone, u.created_at
    FROM users u
    WHERE u.id = p_id;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sp_update_user(
    p_id INTEGER,
    p_fullname VARCHAR,
    p_email VARCHAR,
    p_phone VARCHAR,
    p_password VARCHAR DEFAULT NULL
)
RETURNS TABLE (
    id INTEGER,
    fullname VARCHAR,
    email VARCHAR,
    phone VARCHAR,
    created_at TIMESTAMP
) AS $$
BEGIN
    IF p_password IS NOT NULL THEN
        UPDATE users
        SET fullname = p_fullname,
            email = p_email,
            phone = p_phone,
            password = p_password
        WHERE id = p_id;
    ELSE
        UPDATE users
        SET fullname = p_fullname,
            email = p_email,
            phone = p_phone
        WHERE id = p_id;
    END IF;

    RETURN QUERY
    SELECT u.id, u.fullname, u.email, u.phone, u.created_at
    FROM users u
    WHERE u.id = p_id;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sp_delete_user(p_id INTEGER)
RETURNS INTEGER AS $$
DECLARE
    rows_deleted INTEGER;
BEGIN
    DELETE FROM users WHERE id = p_id;
    GET DIAGNOSTICS rows_deleted = ROW_COUNT;
    RETURN rows_deleted;
END;
$$ LANGUAGE plpgsql; 