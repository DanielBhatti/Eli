begin transaction;
ALTER DATABASE eli OWNER TO eli_owner;
create schema if not exists stage_finance;
create table stage_finance.aaa_transaction(
    aaa_transaction_id UUID not null PRIMARY KEY DEFAULT uuidv7(),
    trans_date date not null,
    description_location text not null,
    amount numeric(9, 2) not null
);
create table stage_finance.boa_transaction(
    boa_transaction_id UUID not null PRIMARY KEY DEFAULT uuidv7(),
    date date not null,
    description text not null,
    location text not null,
    amount numeric(9, 2) not null
);
create table stage_finance.capital_one_transaction(
    capital_one_transaction_id UUID not null PRIMARY KEY DEFAULT uuidv7(),
    trans_date date not null,
    post_date date null,
    description text not null,
    amount numeric(9, 2) not null,
    transaction_type text not null
);
create table stage_finance.citi_transaction(
    citi_transaction_id UUID not null PRIMARY KEY DEFAULT uuidv7(),
    date date not null,
    description text not null,
    debit numeric(9, 2) not null,
    credit numeric(9, 2) not null,
    category text not null
);
create table stage_finance.discover_transaction(
    discover_transaction_id UUID not null PRIMARY KEY DEFAULT uuidv7(),
    trans_date date not null,
    description text not null,
    amount numeric(9, 2) not null,
    category text not null
);
create table stage_finance.fidelity_transaction(
    fidelity_transaction_id UUID not null PRIMARY KEY DEFAULT uuidv7(),
    transaction_type text not null,
    category text not null,
    label text not null,
    transaction_date date not null,
    posted_date date not null,
    merchant text not null,
    amount numeric(9, 2) not null
);
create table stage_finance.td_transaction(
    td_transaction_id UUID not null PRIMARY KEY DEFAULT uuidv7(),
    date date not null,
    posted_date date not null,
    reference_number text not null,
    activity_type text not null,
    status text not null,
    card_number text not null,
    merchant_category text not null,
    merchant_name text not null,
    merchant_city text not null,
    merchant_state_province text not null,
    merchant_postal_code text not null,
    amount numeric(9, 2) not null,
    rewards int not null,
    transaction_type text not null
);
create table stage_finance.us_bank_transaction(
    us_bank_transaction_id UUID not null PRIMARY KEY DEFAULT uuidv7(),
    post_date date not null,
    trans_date date not null,
    ref_number text not null,
    transaction_description text not null,
    amount numeric(9, 2) not null,
    transaction_type text not null
);
create table stage_finance.valley_bank_transaction(
    valley_bank_transaction_id UUID not null PRIMARY KEY DEFAULT uuidv7(),
    post_date date not null,
    trans_date date not null,
    ref_number text not null,
    transaction_description text not null,
    amount numeric(9, 2) not null,
    transaction_type text not null
);
CREATE SCHEMA IF NOT EXISTS finance;
CREATE TABLE finance.purchase_channel (
    purchase_channel_id UUID PRIMARY KEY DEFAULT uuidv7(),
    purchase_channel_name TEXT NOT NULL UNIQUE
);
CREATE TABLE finance.credit_card_transaction (
    credit_card_transaction_id UUID PRIMARY KEY DEFAULT uuidv7(),
    transaction_date DATE NOT NULL,
    description TEXT NOT NULL,
    purchase_channel_id UUID NOT NULL,
    country TEXT,
    state_or_province TEXT,
    amount NUMERIC(18, 6) NOT NULL,
    foreign key (purchase_channel_id) references finance.purchase_channel(purchase_channel_id)
);
CREATE TABLE finance.category (
    category_id UUID PRIMARY KEY DEFAULT uuidv7(),
    category_name TEXT NOT NULL UNIQUE
);
CREATE TABLE finance.label (
    label_id UUID PRIMARY KEY DEFAULT uuidv7(),
    label_name TEXT NOT NULL UNIQUE
);
CREATE TABLE finance.credit_card_transaction_category (
    credit_card_transaction_category_id UUID PRIMARY KEY DEFAULT uuidv7(),
    transaction_id UUID NOT NULL,
    category_id UUID NOT NULL,
    UNIQUE (transaction_id, category_id),
    FOREIGN KEY (transaction_id) REFERENCES finance.credit_card_transaction(credit_card_transaction_id),
    FOREIGN KEY (category_id) REFERENCES finance.category(category_id)
);
CREATE TABLE finance.credit_card_transaction_label (
    credit_card_transaction_label_id UUID PRIMARY KEY DEFAULT uuidv7(),
    transaction_id UUID NOT NULL,
    label_id UUID NOT NULL,
    UNIQUE (transaction_id, label_id),
    FOREIGN KEY (transaction_id) REFERENCES finance.credit_card_transaction(credit_card_transaction_id),
    FOREIGN KEY (label_id) REFERENCES finance.label(label_id)
);
commit;