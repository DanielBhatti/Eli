-- select
-- 'alter table ' || table_schema || '.' || table_name || ' add column is_excluded_from_processing boolean not null default false;',
-- 'alter table ' || table_schema || '.' || table_name || ' drop column is_excluded_from_processing;'
-- from information_schema.tables
-- where table_schema = 'stage_finance'
-- order by table_name;
set role eli_owner;
begin transaction;
alter table stage_finance.aaa_transaction
add column is_excluded_from_processing boolean not null default false;
alter table stage_finance.boa_transaction
add column is_excluded_from_processing boolean not null default false;
alter table stage_finance.capital_one_transaction
add column is_excluded_from_processing boolean not null default false;
alter table stage_finance.citi_transaction
add column is_excluded_from_processing boolean not null default false;
alter table stage_finance.discover_transaction
add column is_excluded_from_processing boolean not null default false;
alter table stage_finance.fidelity_transaction
add column is_excluded_from_processing boolean not null default false;
alter table stage_finance.td_transaction
add column is_excluded_from_processing boolean not null default false;
alter table stage_finance.us_bank_transaction
add column is_excluded_from_processing boolean not null default false;
alter table stage_finance.valley_bank_transaction
add column is_excluded_from_processing boolean not null default false;
commit;