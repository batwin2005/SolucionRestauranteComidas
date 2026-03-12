document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('factura-form');
    if (!form) return;

    form.addEventListener('submit', async function (e) {
        e.preventDefault();
        const data = {
            ClienteNombre: document.querySelector('[name="ClienteNombre"]').value,
            Total: parseFloat(document.querySelector('[name="Total"]').value) || 0
        };

        const res = await fetch('/api/factura', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });

        if (res.ok) {
            const created = await res.json();
            alert('Factura creada con id: ' + (created.id ?? created.Id ?? 'desconocido'));
            window.location.href = '/Factura/Index';
        } else {
            const txt = await res.text();
            alert('Error: ' + txt);
        }
    });
});document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('factura-form');
    if (!form) return;

    form.addEventListener('submit', async function (e) {
        e.preventDefault();
        const data = {
            ClienteNombre: document.querySelector('[name="ClienteNombre"]').value,
            Total: parseFloat(document.querySelector('[name="Total"]').value) || 0
        };

        const res = await fetch('/api/factura', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });

        if (res.ok) {
            const created = await res.json();
            alert('Factura creada con id: ' + (created.id ?? created.Id ?? 'desconocido'));
            window.location.href = '/Factura/Index';
        } else {
            const txt = await res.text();
            alert('Error: ' + txt);
        }
    });
});