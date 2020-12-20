﻿// Assumes you've already included Stripe.js!
const stripe = Stripe('pk_test_51HrzKFJWAkWQReajLesoTWvF3tGF0iOZCpDVpkwJrcUrhcLHf3jCriPbMvIG9dLLZKTFQnShS3RUis2koJ6i4qGJ00MgENXbP9');
const myForm = document.querySelector('.my-form');
myForm.addEventListener('submit', handleForm);

async function handleForm(event) {
    event.preventDefault();

    const accountResult = await stripe.createToken('account', {
        business_type: 'company',
        company: {
            name: document.querySelector('.inp-company-name').value,
            address: {
                line1: document.querySelector('.inp-company-street-address1').value,
                city: document.querySelector('.inp-company-city').value,
                state: document.querySelector('.inp-company-state').value,
                postal_code: document.querySelector('.inp-company-zip').value,
            },
        },
        tos_shown_and_accepted: true,
    });
    alert(accountResult);

    const personResult = await stripe.createToken('person', {
        person: {
            first_name: document.querySelector('.inp-person-first-name').value,
            last_name: document.querySelector('.inp-person-last-name').value,
            address: {
                line1: document.querySelector('.inp-person-street-address1').value,
                city: document.querySelector('.inp-person-city').value,
                state: document.querySelector('.inp-person-state').value,
                postal_code: document.querySelector('.inp-person-zip').value,
            },
        },
    });

    if (accountResult.token && personResult.token) {
        document.querySelector('#token-account').value = accountResult.token.id;
        document.querySelector('#token-person').value = personResult.token.id;
        myForm.submit();
    }
}