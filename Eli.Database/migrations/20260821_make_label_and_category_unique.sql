set role eli_owner;
begin transaction;
lock table finance.credit_card_transaction,
finance.credit_card_transaction_category,
finance.credit_card_transaction_label in access exclusive mode;
-- A scalar category_id/label_id cannot represent missing or multiple mappings.
-- Abort without changing the schema so those transactions can be resolved first.
do $$
declare invalid_category_count integer;
invalid_label_count integer;
begin
select count(*) into invalid_category_count
from (
        select t.credit_card_transaction_id
        from finance.credit_card_transaction t
            left join finance.credit_card_transaction_category mapping on mapping.transaction_id = t.credit_card_transaction_id
        group by t.credit_card_transaction_id
        having count(mapping.category_id) <> 1
    ) invalid_transactions;
select count(*) into invalid_label_count
from (
        select t.credit_card_transaction_id
        from finance.credit_card_transaction t
            left join finance.credit_card_transaction_label mapping on mapping.transaction_id = t.credit_card_transaction_id
        group by t.credit_card_transaction_id
        having count(mapping.label_id) <> 1
    ) invalid_transactions;
if invalid_category_count > 0
or invalid_label_count > 0 then raise exception 'Cannot migrate: % transactions do not have exactly one category and % do not have exactly one label.',
invalid_category_count,
invalid_label_count;
end if;
end $$;
alter table finance.credit_card_transaction
add column category_id uuid,
    add column label_id uuid;
update finance.credit_card_transaction t
set category_id = mapping.category_id
from finance.credit_card_transaction_category mapping
where mapping.transaction_id = t.credit_card_transaction_id;
update finance.credit_card_transaction t
set label_id = mapping.label_id
from finance.credit_card_transaction_label mapping
where mapping.transaction_id = t.credit_card_transaction_id;
alter table finance.credit_card_transaction
alter column category_id
set not null,
    alter column label_id
set not null,
    add constraint credit_card_transaction_category_id_fkey foreign key (category_id) references finance.category(category_id),
    add constraint credit_card_transaction_label_id_fkey foreign key (label_id) references finance.label(label_id);
drop table finance.credit_card_transaction_category;
drop table finance.credit_card_transaction_label;
commit;