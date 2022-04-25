/* CREATE TABLES */
DROP TABLE IF EXISTS work_type CASCADE;
CREATE TABLE work_type (
    id SERIAL PRIMARY KEY NOT NULL,
    name VARCHAR(255) NOT NULL
);

DROP TABLE IF EXISTS application_status CASCADE;
CREATE TABLE application_status (
    id SERIAL PRIMARY KEY NOT NULL,
    name VARCHAR(255) NOT NULL
);

DROP TABLE IF EXISTS application CASCADE;
CREATE TABLE application (
    id SERIAL PRIMARY KEY NOT NULL,
    user_id INTEGER NOT NULL,
    job_title VARCHAR(255) NOT NULL,
    work_type_id INTEGER NOT NULL,
    company_name VARCHAR(255) NOT NULL,
    submission_date DATE,
    status_id INTEGER NOT NULL,
    wanted_salary MONEY,
    accepted_salary MONEY,
    start_date DATE,
    commentary VARCHAR(500),

    FOREIGN KEY (work_type_id) REFERENCES work_type (id)
        ON DELETE SET DEFAULT,
    FOREIGN KEY (status_id) REFERENCES application_status (id)
        ON DELETE SET DEFAULT
);

/* INSERT STATEMENTS */
INSERT INTO work_type (name) VALUES ('Remote');
INSERT INTO work_type (name) VALUES ('OnSite');
INSERT INTO work_type (name) VALUES ('Hybrid');

INSERT INTO application_status (name) VALUES ('Accepted');
INSERT INTO application_status (name) VALUES ('Pending');
INSERT INTO application_status (name) VALUES ('Declined');

INSERT INTO application (user_id, job_title, work_type_id, company_name, submission_date, status_id, wanted_salary, accepted_salary, start_date, commentary)
VALUES (5, 'Software Developer', (SELECT id FROM work_type WHERE name LIKE 'Remote'), 'Fujitsu Technology Solutions GmbH', '2022-02-01', (SELECT id FROM application_status WHERE name LIKE 'Accepted'), 60000, 60000, '2022-04-01', NULL);