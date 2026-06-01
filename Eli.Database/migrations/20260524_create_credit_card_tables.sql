begin transaction;
create table finance.credit_card(
  credit_card_id uuid not null primary key default uuidv7(),
  credit_card_name text not null unique constraint credit_card_credit_card_name_le_100 check (char_length(credit_card_name) <= 50)
);
alter table finance.credit_card_transaction
add column credit_card_id uuid not null references finance.credit_card(credit_card_id);
alter table finance.credit_card_transaction
add column source_uuid uuid not null;
commit;