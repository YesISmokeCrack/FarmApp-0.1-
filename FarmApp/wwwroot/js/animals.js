// animals.js

const API_BASE = 'https://localhost:7113/api'; // Замените порт на свой!

// Загрузка списка животных
async function loadAnimals() {
    try {
        const response = await fetch(`${API_BASE}/animals`);
        if (!response.ok) throw new Error(`Ошибка HTTP: ${response.status}`);
        const animals = await response.json();

        const tbody = document.getElementById('animalsBody');
        tbody.innerHTML = '';

        animals.forEach(animal => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${animal.id}</td>
                <td>${animal.name}</td>
                <td>${animal.type === 'Cow' ? 'Корова' : animal.type}</td>
                <td>${animal.breed || '-'}</td>
                <td>${new Date(animal.birthDate).toLocaleDateString()}</td>
                <td>${animal.gender === 'Female' ? '♀' : '♂'}</td>
                <td>${animal.healthStatus}</td>
                <td>
                    <button class="btn btn-sm btn-danger" onclick="deleteAnimal(${animal.id})">
                        <i class="fas fa-trash"></i>
                    </button>
                </td>
            `;
            tbody.appendChild(row);
        });
    } catch (error) {
        console.error('Ошибка загрузки животных:', error);
        alert('Не удалось загрузить список животных');
    }
}

// Обработка формы добавления
document.getElementById('animalForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    const newAnimal = {
        name: document.getElementById('name').value,
        type: document.getElementById('type').value,
        breed: document.getElementById('breed').value,
        birthDate: document.getElementById('birthDate').value,
        gender: document.getElementById('gender').value,
        healthStatus: document.getElementById('healthStatus').value,
        lastVaccinationDate: null
    };

    try {
        const response = await fetch(`${API_BASE}/animals`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(newAnimal)
        });

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(errorText);
        }

        document.getElementById('animalForm').reset();
        loadAnimals();
        alert('Животное добавлено!');
    } catch (error) {
        console.error('Ошибка при добавлении:', error);
        alert('Ошибка при добавлении: ' + error.message);
    }
});

// Удаление животного
async function deleteAnimal(id) {
    if (!confirm('Вы уверены, что хотите удалить животное?')) return;

    try {
        const response = await fetch(`${API_BASE}/animals/${id}`, {
            method: 'DELETE'
        });
        if (!response.ok) throw new Error('Ошибка удаления');
        loadAnimals();
    } catch (error) {
        console.error('Ошибка удаления:', error);
        alert('Не удалось удалить животное');
    }
}

// Загрузка при открытии страницы
document.addEventListener('DOMContentLoaded', loadAnimals);