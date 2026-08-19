set role eli_owner;
begin transaction;
alter table stage_finance.aaa_transaction
add column file_name text not null;
alter table stage_finance.boa_transaction
add column file_name text not null;
alter table stage_finance.capital_one_transaction
add column file_name text not null;
alter table stage_finance.citi_transaction
add column file_name text not null;
alter table stage_finance.discover_transaction
add column file_name text not null;
alter table stage_finance.fidelity_transaction
add column file_name text not null;
alter table stage_finance.td_transaction
add column file_name text not null;
alter table stage_finance.us_bank_transaction
add column file_name text not null;
alter table stage_finance.valley_bank_transaction
add column file_name text not null;
insert into finance.purchase_channel(purchase_channel_name)
values ('Unknown');
insert into finance.category(category_name)
values ('Unknown');
update finance.credit_card_transaction
set country = 'Unknown'
where country is null;
update finance.credit_card_transaction
set state_or_province = 'Unknown'
where state_or_province is null;
alter table finance.credit_card_transaction
alter column country
set default 'Unknown';
alter table finance.credit_card_transaction
alter column country
set not null;
alter table finance.credit_card_transaction
alter column state_or_province
set default 'Unknown';
alter table finance.credit_card_transaction
alter column state_or_province
set not null;
commit;