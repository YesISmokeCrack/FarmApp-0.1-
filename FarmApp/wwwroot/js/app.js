const API = '/api';
const today = () => new Date().toISOString().slice(0, 10);
const icons = {
    farm: '<path d="M3 20V9l9-6 9 6v11h-6v-6H9v6H3Z"/><path d="M7 10h.01M12 10h.01M17 10h.01"/>',
    home: '<path d="m3 10 9-7 9 7v10a1 1 0 0 1-1 1h-5v-6H9v6H4a1 1 0 0 1-1-1V10Z"/>',
    animal: '<path d="M8 11c-2 0-3-2-3-4 0-1 .6-2 1.5-2S9 7 9 9m7 2c2 0 3-2 3-4 0-1-.6-2-1.5-2S15 7 15 9M6 14c0-3 2.7-5 6-5s6 2 6 5-2.7 6-6 6-6-3-6-6Zm3 1h.01M15 15h.01M10 18c1.2.8 2.8.8 4 0"/>',
    milk: '<path d="M9 3h6l1 4 3 3v10a1 1 0 0 1-1 1H6a1 1 0 0 1-1-1V10l3-3 1-4Z"/><path d="M8 7h8M5 11h14M12 14v4"/>',
    feed: '<path d="M12 21c-4-4-4-10 0-16 4 6 4 12 0 16Z"/><path d="M12 21c-3-3-6-4-9-4 1 3 4 5 9 4Zm0-6c2-3 5-5 9-5-1 4-4 7-9 7"/>',
    product: '<path d="M6 3h12l2 5-2 13H6L4 8l2-5Z"/><path d="M4 8h16M9 12h6"/>',
    sale: '<path d="M4 4h2l2.2 11h9.9l2-8H7"/><circle cx="10" cy="19" r="1"/><circle cx="17" cy="19" r="1"/>',
    search: '<circle cx="10.8" cy="10.8" r="6.8"/><path d="m16 16 5 5"/>',
    plus: '<path d="M12 5v14M5 12h14"/>',
    edit: '<path d="m15 5 4 4M4 20l4-.8L19 8a2.1 2.1 0 0 0-3-3L5 16l-1 4Z"/>',
    trash: '<path d="M4 7h16M10 11v6m4-6v6M6 7l1 14h10l1-14M9 7V4h6v3"/>',
    consume: '<path d="M12 3v12m-5-5 5 5 5-5M5 20h14"/>',
    clock: '<circle cx="12" cy="12" r="9"/><path d="M12 7v5l3 2"/>',
    arrow: '<path d="M5 12h14m-6-6 6 6-6 6"/>',
    calendar: '<rect x="3" y="5" width="18" height="16" rx="2"/><path d="M16 3v4M8 3v4M3 11h18"/>',
    menu: '<path d="M4 7h16M4 12h16M4 17h16"/>'
};
const icon = (name, cls = '') => `<svg class="${cls}" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true">${icons[name] || icons.farm}</svg>`;
const escapeHtml = value => String(value ?? '').replace(/[&<>"']/g, char => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[char]));
const toDate = value => value ? new Date(value).toLocaleDateString('ru-RU') : '—';
const number = value => new Intl.NumberFormat('ru-RU', { maximumFractionDigits: 2 }).format(Number(value || 0));
const currency = value => new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB', maximumFractionDigits: 0 }).format(Number(value || 0));

const sections = {
    animals: { title: 'Животные', subtitle: 'Карточки поголовья, здоровье и вакцинация', endpoint: 'animals', icon: 'animal', singular: 'животное', fields: [
        { key: 'name', label: 'Кличка или номер', required: true, max: 50 },
        { key: 'type', label: 'Вид животного', type: 'select', required: true, options: [['Cow', 'Корова'], ['Sheep', 'Овца'], ['Goat', 'Коза'], ['Pig', 'Свинья'], ['Horse', 'Лошадь'], ['Other', 'Другое']] },
        { key: 'breed', label: 'Порода' },
        { key: 'birthDate', label: 'Дата рождения', type: 'date', required: true, default: today },
        { key: 'gender', label: 'Пол', type: 'select', required: true, options: [['Female', 'Самка'], ['Male', 'Самец']] },
        { key: 'healthStatus', label: 'Состояние', type: 'select', required: true, options: [['Healthy', 'Здорово'], ['Recovering', 'На восстановлении'], ['Sick', 'Болеет']] },
        { key: 'lastVaccinationDate', label: 'Последняя вакцинация', type: 'date' }
    ] },
    milk: { title: 'Надои молока', subtitle: 'Ежедневные записи по животным', endpoint: 'milkrecords', icon: 'milk', singular: 'запись о надое', fields: [
        { key: 'animalId', label: 'Животное', type: 'animals', required: true },
        { key: 'date', label: 'Дата', type: 'date', required: true, default: today },
        { key: 'quantity', label: 'Объём, л', type: 'number', required: true, min: 0.1, step: 0.1 },
        { key: 'fatPercentage', label: 'Жирность, %', type: 'number', required: true, min: 0, max: 10, step: 0.1 },
        { key: 'proteinPercentage', label: 'Белок, %', type: 'number', required: true, min: 0, max: 10, step: 0.1 }
    ] },
    feeds: { title: 'Корма', subtitle: 'Запасы, сроки годности и расход кормов', endpoint: 'feeds', icon: 'feed', singular: 'корм', fields: [
        { key: 'name', label: 'Название', required: true, max: 100 },
        { key: 'type', label: 'Категория', type: 'select', required: true, options: [['Hay', 'Сено'], ['Concentrate', 'Комбикорм'], ['Grain', 'Зерно'], ['Silage', 'Силос'], ['Other', 'Другое']] },
        { key: 'quantity', label: 'Количество', type: 'number', required: true, min: 0, step: 0.1 },
        { key: 'unit', label: 'Единица измерения', type: 'select', required: true, options: [['kg', 'кг'], ['l', 'л'], ['t', 'т'], ['pcs', 'шт.']] },
        { key: 'expiryDate', label: 'Срок годности', type: 'date' }
    ] },
    products: { title: 'Продукция', subtitle: 'Произведённая продукция и сроки хранения', endpoint: 'products', icon: 'product', singular: 'продукт', fields: [
        { key: 'name', label: 'Название продукта', required: true, max: 100 },
        { key: 'type', label: 'Категория', type: 'select', required: true, options: [['Milk', 'Молочная продукция'], ['Meat', 'Мясо'], ['Eggs', 'Яйца'], ['Other', 'Другое']] },
        { key: 'quantity', label: 'Количество', type: 'number', required: true, min: 0, step: 0.1, createOnly: true },
        { key: 'unit', label: 'Единица измерения', type: 'select', required: true, options: [['l', 'л'], ['kg', 'кг'], ['pcs', 'шт.'], ['pack', 'упак.']] },
        { key: 'productionDate', label: 'Дата производства', type: 'date', required: true, default: today },
        { key: 'expiryDate', label: 'Годен до', type: 'date', required: true },
        { key: 'animalId', label: 'Связанное животное', type: 'animals', optional: true }
    ] },
    sales: { title: 'Продажи', subtitle: 'История реализации и выручка', endpoint: 'sales', icon: 'sale', singular: 'продажу', createOnly: true, fields: [
        { key: 'productId', label: 'Продукт', type: 'products', required: true },
        { key: 'customer', label: 'Покупатель', required: true, max: 100 },
        { key: 'quantity', label: 'Количество', type: 'number', required: true, min: 0.1, step: 0.1 },
        { key: 'pricePerUnit', label: 'Цена за единицу, ₽', type: 'number', required: true, min: 0.01, step: 0.01 },
        { key: 'saleDate', label: 'Дата продажи', type: 'date', required: true, default: today }
    ] },
    accounts: { title: 'Работники', subtitle: 'Учётные записи сотрудников фермы', endpoint: 'accounts', icon: 'farm', singular: 'учётную запись работника', fields: [
        { key: 'userName', label: 'Логин', required: true, min: 3, max: 32, hint: 'Латинские буквы, цифры и символы . _ - @ +' },
        { key: 'password', label: 'Временный пароль', type: 'password', required: true, min: 8, max: 100, hint: 'Не менее 8 символов, заглавная и строчная буквы, цифра.' }
    ] }
};
const navItems = [
    { id: 'dashboard', title: 'Обзор', icon: 'home' },
    { id: 'animals', title: 'Животные', icon: 'animal' },
    { id: 'milk', title: 'Надои молока', icon: 'milk' },
    { id: 'feeds', title: 'Корма', icon: 'feed' },
    { id: 'products', title: 'Продукция', icon: 'product' },
    { id: 'sales', title: 'Продажи', icon: 'sale' },
    { id: 'accounts', title: 'Работники', icon: 'farm', adminOnly: true }
];
let activeSection = 'dashboard';
let authUser = null;
let datasets = {};
let visibleRows = [];

async function request(path, options = {}) {
    const response = await fetch(`${API}${path}`, { ...options, headers: { ...(options.body ? { 'Content-Type': 'application/json' } : {}), ...options.headers } });
    if (response.status === 401 && authUser) {
        authUser = null;
        renderAuth('login', 'Срок действия входа истёк. Войдите снова.');
    }
    if (!response.ok) {
        const body = await response.text();
        const message = authErrorMessage(body, `Ошибка сервера (${response.status})`);
        throw new Error(message || `Ошибка сервера (${response.status})`);
    }
    if (response.status === 204) return null;
    const text = await response.text();
    return text ? JSON.parse(text) : null;
}
function showToast(message, kind = 'success') {
    const toast = document.createElement('div');
    toast.className = `toast ${kind}`;
    toast.textContent = message;
    document.getElementById('toastStack').append(toast);
    window.setTimeout(() => toast.remove(), 4200);
}
function labelFor(key, value) {
    const maps = {
        type: { Cow: 'Корова', Sheep: 'Овца', Goat: 'Коза', Pig: 'Свинья', Horse: 'Лошадь', Hay: 'Сено', Concentrate: 'Комбикорм', Grain: 'Зерно', Silage: 'Силос', Milk: 'Молочная продукция', Meat: 'Мясо', Eggs: 'Яйца', Other: 'Другое' },
        gender: { Female: 'Самка', Male: 'Самец' }, healthStatus: { Healthy: 'Здорово', Recovering: 'На восстановлении', Sick: 'Болеет' },
        unit: { kg: 'кг', l: 'л', t: 'т', pcs: 'шт.', pack: 'упак.' }
    };
    return maps[key]?.[value] || value || '—';
}
function statusBadge(row) {
    const status = row.healthStatus;
    if (!status) return '';
    const cls = status === 'Healthy' ? '' : status === 'Recovering' ? ' badge-warn' : ' badge-danger';
    return `<span class="badge${cls}">${escapeHtml(labelFor('healthStatus', status))}</span>`;
}
function authErrorMessage(body, fallback) {
    if (!body) return fallback;
    try {
        const result = JSON.parse(body);
        if (result.message) return result.message;
        if (result.title) return result.title;
        if (result.errors) return Object.values(result.errors).flat().join(' ');
    } catch { /* The server may return a plain-text error. */ }
    return body;
}
function renderAuth(mode, message = '') {
    const isSetup = mode === 'setup';
    const host = document.getElementById('authHost');
    host.innerHTML = `<section class="auth-screen"><div class="auth-art"><a class="auth-brand" href="#"><span class="brand-mark">${icon('farm')}</span><span><span class="auth-brand-title">FarmApp</span><span class="auth-brand-subtitle" style="display:block">Управление хозяйством</span></span></a><div class="auth-art-copy"><div class="eyebrow">${isSetup ? 'Первый запуск' : 'Рабочее пространство'}</div><h1>${isSetup ? 'Создайте доступ к своей ферме' : 'Рады видеть вас снова'}</h1><p>${isSetup ? 'Сначала создайте учётную запись администратора. Затем администратор сможет добавить работников.' : 'Войдите в систему, чтобы открыть журналы и данные фермы.'}</p></div><div class="auth-footnote">Надёжный учёт хозяйства начинается с команды.</div></div><div class="auth-form-side"><form class="auth-card" id="authForm"><span class="auth-card-mark">${icon('farm')}</span><h2>${isSetup ? 'Создать администратора' : 'Вход в FarmApp'}</h2><p class="auth-card-intro">${isSetup ? 'Эта первая учётная запись получит права администратора.' : 'Введите логин и пароль, выданные администратором.'}</p><div class="auth-field"><label for="authUserName">Логин</label><input id="authUserName" name="userName" autocomplete="username" minlength="3" maxlength="32" required autofocus>${isSetup ? '<div class="auth-hint">Латинские буквы, цифры и символы . _ - @ +.</div>' : ''}</div><div class="auth-field"><label for="authPassword">Пароль</label><input id="authPassword" name="password" type="password" autocomplete="${isSetup ? 'new-password' : 'current-password'}" minlength="8" maxlength="100" required>${isSetup ? '<div class="auth-hint">Не менее 8 символов: заглавная и строчная латинские буквы, а также цифра.</div>' : ''}</div>${isSetup ? '<div class="auth-field"><label for="authConfirmPassword">Повторите пароль</label><input id="authConfirmPassword" name="confirmPassword" type="password" autocomplete="new-password" minlength="8" maxlength="100" required></div>' : ''}<button class="button button-primary auth-submit" type="submit">${isSetup ? 'Создать администратора' : 'Войти в систему'}</button><div class="auth-error" id="authError" role="alert">${escapeHtml(message)}</div><div class="auth-security">${icon('clock')}Вход защищён. После пяти неверных попыток он временно блокируется.</div></form></div></section>`;
    const form = document.getElementById('authForm');
    form.addEventListener('submit', async event => {
        event.preventDefault();
        if (!form.reportValidity()) return;
        const userName = document.getElementById('authUserName').value.trim();
        const password = document.getElementById('authPassword').value;
        if (isSetup && password !== document.getElementById('authConfirmPassword').value) {
            document.getElementById('authError').textContent = 'Пароли не совпадают.';
            return;
        }
        const button = form.querySelector('button[type="submit"]');
        button.disabled = true;
        button.textContent = 'Подождите…';
        try {
            const response = await fetch(`/api/auth/${isSetup ? 'setup' : 'login'}`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ userName, password })
            });
            const body = await response.text();
            if (!response.ok) throw new Error(authErrorMessage(body, 'Не удалось войти.'));
            authUser = body ? JSON.parse(body) : null;
            host.innerHTML = '';
            navMarkup();
            updateTopbar();
            if (!location.hash || (location.hash === '#accounts' && !authUser?.isAdmin)) location.hash = '#dashboard';
            await navigate();
        } catch (error) {
            document.getElementById('authError').textContent = error.message || 'Ошибка входа. Проверьте соединение и попробуйте ещё раз.';
            button.disabled = false;
            button.textContent = isSetup ? 'Создать администратора' : 'Войти в систему';
        }
    });
}
async function startApp() {
    try {
        const setupResponse = await fetch('/api/auth/setup-status');
        if (!setupResponse.ok) throw new Error('Не удалось проверить настройку учётных записей.');
        const setup = await setupResponse.json();
        if (setup.setupRequired) {
            renderAuth('setup');
            return;
        }
        const sessionResponse = await fetch('/api/auth/me');
        if (sessionResponse.status === 401) {
            renderAuth('login');
            return;
        }
        if (!sessionResponse.ok) throw new Error('Не удалось проверить учётную запись.');
        authUser = await sessionResponse.json();
        navMarkup();
        updateTopbar();
        if (location.hash === '#accounts' && !authUser.isAdmin) location.hash = '#dashboard';
        await navigate();
    } catch (error) {
        renderAuth('login', error.message || 'Не удалось подключиться к приложению.');
    }
}
document.getElementById('logoutButton').addEventListener('click', async () => {
    try {
        await fetch('/api/auth/logout', { method: 'POST' });
    } finally {
        authUser = null;
        renderAuth('login', 'Вы вышли из системы.');
    }
});
function navMarkup() {
    const items = navItems.filter(item => !item.adminOnly || authUser?.isAdmin);
    document.getElementById('navigation').innerHTML = items.map(item => `<a class="nav-link${activeSection === item.id ? ' active' : ''}" href="#${item.id}" data-section="${item.id}">${icon(item.icon)}<span>${item.title}</span></a>`).join('');
}
function updateTopbar() {
    const item = navItems.find(entry => entry.id === activeSection);
    document.getElementById('topbarTitle').textContent = item?.title || 'Обзор';
    document.getElementById('todayLabel').textContent = new Intl.DateTimeFormat('ru-RU', { weekday: 'long', day: 'numeric', month: 'long' }).format(new Date());
    document.getElementById('userLabel').textContent = authUser?.userName || '';
    document.getElementById('roleLabel').textContent = authUser?.isAdmin ? 'Администратор' : 'Работник';
    document.getElementById('userBadge').textContent = (authUser?.userName || 'Ф').slice(0, 1).toLocaleUpperCase('ru');
}
function dashboardMarkup() {
    const animals = datasets.animals || [];
    const feeds = datasets.feeds || [];
    const products = datasets.products || [];
    const sales = datasets.sales || [];
    const revenue = sales.reduce((sum, item) => sum + Number(item.totalAmount || 0), 0);
    const milkRecords = datasets.milk || [];
    const milkToday = milkRecords.filter(item => String(item.date).slice(0, 10) === today()).reduce((sum, item) => sum + Number(item.quantity || 0), 0);
    return `<div class="page-heading"><div><div class="eyebrow">Панель управления</div><h1>Добро пожаловать на ферму</h1><p>Все важные показатели хозяйства — в одном месте.</p></div><button class="button button-primary" data-action="create-animal">${icon('plus')}Добавить животное</button></div>
        <section class="hero"><div><div class="eyebrow" style="color:#c7e4cf">Управление хозяйством</div><h2>Ферма под контролем — каждый день</h2><p>Следите за поголовьем, запасами, надоями и продажами в удобной системе учёта.</p></div><a class="button" href="#animals">Открыть журнал ${icon('arrow')}</a><span class="hero-mark" aria-hidden="true">🌿</span></section>
        <section class="stats-grid">
            ${statCard('animal', 'Поголовье', animals.length, 'животных в учёте')}
            ${statCard('milk', 'Надой сегодня', `${number(milkToday)} л`, 'по зарегистрированным записям')}
            ${statCard('feed', 'Запасы кормов', feeds.length, 'позиций на складе')}
            ${statCard('sale', 'Выручка', currency(revenue), `${sales.length} продаж в журнале`)}
        </section>
        <section class="dashboard-bottom"><div class="panel"><div class="panel-heading"><div><h2>Разделы фермы</h2><p>Быстрый доступ к рабочим журналам</p></div></div><div class="panel-body"><div class="quick-list">${navItems.filter(item => item.id !== 'dashboard').map(item => `<a class="quick-link" href="#${item.id}"><span class="module-icon">${icon(item.icon)}</span>${item.title}</a>`).join('')}</div></div></div>
        <div class="panel"><div class="panel-heading"><div><h2>Сводка хозяйства</h2><p>Текущая ситуация</p></div></div><div class="panel-body"><div class="alert-line"><strong>Животные</strong><span>${animals.length} в хозяйстве</span></div><div class="alert-line"><strong>Продукция</strong><span>${products.length} наименований</span></div><div class="alert-line"><strong>Запасы кормов</strong><span>${feeds.length} позиций</span></div><div class="alert-line"><strong>Продажи</strong><span>${sales.length} операций</span></div></div></div></section>`;
}
function statCard(iconName, title, value, note) {
    return `<article class="stat-card"><span class="stat-icon">${icon(iconName)}</span><div><div class="stat-title">${escapeHtml(title)}</div><div class="stat-value">${escapeHtml(value)}</div><div class="stat-note">${escapeHtml(note)}</div></div></article>`;
}
function formatCell(sectionId, key, row) {
    const value = row[key];
    if (key === 'name' || key === 'productName' || key === 'animalName' || key === 'customer' || key === 'userName') return `<span class="cell-main">${escapeHtml(value || '—')}</span>`;
    if (key === 'role') return '<span class="role-pill">Работник</span>';
    if (key.toLowerCase().includes('date')) return escapeHtml(toDate(value));
    if (key === 'healthStatus') return statusBadge(row);
    if (key === 'gender') return escapeHtml(labelFor(key, value));
    if (key === 'type' || key === 'unit') return escapeHtml(labelFor(key, value));
    if (key === 'animalId') return escapeHtml(row.animalName || '—');
    if (key === 'productId') return escapeHtml(row.productName || '—');
    if (key === 'quantity') return `${escapeHtml(number(value))}${sectionId === 'milk' ? ' л' : row.unit ? ` ${escapeHtml(labelFor('unit', row.unit))}` : ''}`;
    if (key === 'fatPercentage' || key === 'proteinPercentage') return `${escapeHtml(number(value))}%`;
    if (key === 'pricePerUnit') return `${escapeHtml(currency(value))}`;
    if (key === 'totalAmount') return `<span class="cell-main">${escapeHtml(currency(value))}</span>`;
    return escapeHtml(value || '—');
}
function tableColumns(sectionId) {
    const columns = {
        animals: [['name', 'Животное'], ['type', 'Вид'], ['breed', 'Порода'], ['birthDate', 'Дата рождения'], ['gender', 'Пол'], ['healthStatus', 'Состояние'], ['lastVaccinationDate', 'Вакцинация']],
        milk: [['animalName', 'Животное'], ['date', 'Дата'], ['quantity', 'Надой'], ['fatPercentage', 'Жирность'], ['proteinPercentage', 'Белок']],
        feeds: [['name', 'Корм'], ['type', 'Категория'], ['quantity', 'Остаток'], ['unit', 'Ед. изм.'], ['expiryDate', 'Годен до']],
        products: [['name', 'Продукт'], ['type', 'Категория'], ['quantity', 'Количество'], ['unit', 'Ед. изм.'], ['productionDate', 'Произведено'], ['expiryDate', 'Годен до'], ['animalName', 'Животное']],
        sales: [['productName', 'Продукт'], ['customer', 'Покупатель'], ['quantity', 'Количество'], ['pricePerUnit', 'Цена за ед.'], ['saleDate', 'Дата продажи'], ['totalAmount', 'Сумма']],
        accounts: [['userName', 'Логин'], ['role', 'Права доступа']]
    };
    return columns[sectionId];
}
function rowActions(sectionId, row) {
    const buttons = [];
    if (sectionId === 'accounts') return `<div class="action-group"><button class="icon-button delete staff-delete" title="Удалить учётную запись работника" aria-label="Удалить работника" data-action="delete" data-id="${escapeHtml(row.id)}">${icon('trash')}</button></div>`;
    if (sectionId === 'feeds') buttons.push(`<button class="icon-button" title="Списать корм" aria-label="Списать корм" data-action="consume" data-id="${row.id}">${icon('consume')}</button>`);
    if (sectionId !== 'sales') buttons.push(`<button class="icon-button" title="Изменить" aria-label="Изменить" data-action="edit" data-id="${row.id}">${icon('edit')}</button><button class="icon-button delete" title="Удалить" aria-label="Удалить" data-action="delete" data-id="${row.id}">${icon('trash')}</button>`);
    return buttons.length ? `<div class="action-group">${buttons.join('')}</div>` : '<span class="cell-sub">Только просмотр</span>';
}
function sectionMarkup(sectionId) {
    const config = sections[sectionId];
    const rows = datasets[sectionId] || [];
    const columns = tableColumns(sectionId);
    const addLabel = sectionId === 'sales' ? 'Оформить продажу' : `Добавить ${config.singular}`;
    return `<div class="page-heading"><div><div class="eyebrow">Журнал хозяйства</div><h1>${config.title}</h1><p>${config.subtitle}</p></div><button class="button button-primary" data-action="create">${icon('plus')}${addLabel}</button></div>
        <section class="panel"><div class="table-toolbar"><label class="search-wrap">${icon('search')}<input class="search-input" id="tableSearch" type="search" placeholder="Поиск по списку…" aria-label="Поиск по списку"></label><span class="result-count" id="resultCount">${rows.length} ${pluralize(sectionId, rows.length)}</span></div>
        <div class="table-scroll"><table><thead><tr>${columns.map(col => `<th>${col[1]}</th>`).join('')}<th style="text-align:right">Действия</th></tr></thead><tbody id="tableBody">${renderRows(sectionId, rows)}</tbody></table></div></section>`;
}
function pluralize(sectionId, count) {
    const variants = { animals: ['животное', 'животных', 'животных'], milk: ['запись', 'записи', 'записей'], feeds: ['позиция', 'позиции', 'позиций'], products: ['позиция', 'позиции', 'позиций'], sales: ['продажа', 'продажи', 'продаж'], accounts: ['работник', 'работника', 'работников'] }[sectionId] || ['запись', 'записи', 'записей'];
    const lastTwo = count % 100;
    const last = count % 10;
    return variants[lastTwo >= 11 && lastTwo <= 14 ? 2 : last === 1 ? 0 : last >= 2 && last <= 4 ? 1 : 2];
}
function renderRows(sectionId, rows) {
    const columns = tableColumns(sectionId);
    if (!rows.length) return `<tr><td colspan="${columns.length + 1}"><div class="table-empty"><strong>Пока нет записей</strong>Добавьте первую запись, чтобы начать вести учёт.<br><button class="button button-primary" data-action="create">${icon('plus')}Добавить</button></div></td></tr>`;
    return rows.map(row => `<tr>${columns.map(([key]) => `<td>${formatCell(sectionId, key, row)}</td>`).join('')}<td>${rowActions(sectionId, row)}</td></tr>`).join('');
}
function applySearch(value) {
    const query = value.trim().toLocaleLowerCase('ru');
    visibleRows = (datasets[activeSection] || []).filter(row => !query || Object.values(row).some(item => String(item ?? '').toLocaleLowerCase('ru').includes(query) || String(labelFor('type', item)).toLocaleLowerCase('ru').includes(query)));
    const body = document.getElementById('tableBody');
    if (body) body.innerHTML = renderRows(activeSection, visibleRows);
    const count = document.getElementById('resultCount');
    if (count) count.textContent = `${visibleRows.length} ${pluralize(activeSection, visibleRows.length)}`;
}
async function loadData(id) {
    if (id === 'dashboard') {
        const results = await Promise.all(['animals', 'milk', 'feeds', 'products', 'sales'].map(async section => [section, await request(`/${sections[section].endpoint}`)]));
        results.forEach(([section, rows]) => { datasets[section] = rows; });
        return;
    }
    datasets[id] = await request(`/${sections[id].endpoint}`);
}
async function navigate() {
    const requested = location.hash.slice(1) || 'dashboard';
    activeSection = navItems.some(item => item.id === requested) && (requested !== 'accounts' || authUser?.isAdmin) ? requested : 'dashboard';
    navMarkup();
    updateTopbar();
    const content = document.getElementById('content');
    content.innerHTML = '<div class="loading"><span class="loading-spinner"></span>Загружаем данные…</div>';
    try {
        await loadData(activeSection);
        content.innerHTML = activeSection === 'dashboard' ? dashboardMarkup() : sectionMarkup(activeSection);
        if (activeSection !== 'dashboard') applySearch('');
    } catch (error) {
        content.innerHTML = `<section class="panel error-state"><strong>Не удалось загрузить данные</strong><p>${escapeHtml(error.message)}</p><button class="button button-plain" id="retryButton">Попробовать снова</button></section>`;
        document.getElementById('retryButton').addEventListener('click', navigate);
        showToast(`Ошибка загрузки: ${error.message}`, 'error');
    }
}
function optionsFor(field) {
    if (field.type === 'animals') return (datasets.animals || []).map(animal => [String(animal.id), `${animal.name} · ${labelFor('type', animal.type)}`]);
    if (field.type === 'products') return (datasets.products || []).map(product => [String(product.id), `${product.name} · ${number(product.quantity)} ${labelFor('unit', product.unit)}`]);
    return field.options || [];
}
function fieldMarkup(field, current, sectionId, isEdit) {
    if (field.createOnly && isEdit) return '';
    const value = current ?? (typeof field.default === 'function' ? field.default() : field.default) ?? '';
    const required = field.required && !field.optional ? 'required' : '';
    let input;
    if (field.type === 'select' || field.type === 'animals' || field.type === 'products') {
        const options = optionsFor(field);
        input = `<select id="field-${field.key}" name="${field.key}" ${required}><option value="">${field.optional ? 'Не выбрано' : 'Выберите…'}</option>${options.map(([key, label]) => `<option value="${escapeHtml(key)}"${String(value) === String(key) ? ' selected' : ''}>${escapeHtml(label)}</option>`).join('')}</select>`;
    } else {
        const type = field.type === 'number' ? 'number' : field.type === 'date' ? 'date' : field.type === 'password' ? 'password' : 'text';
        const isTextField = type === 'text' || type === 'password';
        const attrs = [required, field.max ? `maxlength="${field.max}"` : '', field.min !== undefined ? `${isTextField ? 'minlength' : 'min'}="${field.min}"` : '', field.max !== undefined && !isTextField ? `max="${field.max}"` : '', field.step ? `step="${field.step}"` : '', field.key === 'quantity' && sectionId === 'products' && isEdit ? 'readonly' : ''].filter(Boolean).join(' ');
        input = `<input id="field-${field.key}" name="${field.key}" type="${type}" value="${escapeHtml(value)}" ${attrs}>`;
    }
    const hintText = field.key === 'quantity' && sectionId === 'products' && isEdit ? 'Количество на складе изменяется через списание или продажу.' : field.hint;
    const hint = hintText ? `<div class="field-hint">${escapeHtml(hintText)}</div>` : '';
    return `<div class="field"><label for="field-${field.key}">${field.label}${field.required && !field.optional ? ' *' : ''}</label>${input}${hint}</div>`;
}
function openForm(sectionId, row = null, custom = null) {
    const config = sections[sectionId];
    const isEdit = Boolean(row);
    const title = custom?.title || `${isEdit ? 'Изменить' : sectionId === 'sales' ? 'Оформить' : 'Добавить'} ${config.singular}`;
    const fields = custom?.fields || config.fields;
    const dialog = document.getElementById('formDialog');
    dialog.innerHTML = `<form id="dataForm" novalidate><div class="modal-header"><div><h2>${escapeHtml(title)}</h2><p>Поля со звёздочкой обязательны для заполнения</p></div><button class="modal-close" type="button" aria-label="Закрыть" data-action="close-modal">×</button></div><div class="modal-body"><div class="form-grid">${fields.map(field => fieldMarkup(field, row?.[field.key], sectionId, isEdit)).join('')}</div><div class="form-error" id="formError" role="alert"></div></div><div class="modal-footer"><button class="button button-plain" type="button" data-action="close-modal">Отмена</button><button class="button button-primary" type="submit">${icon('plus')}${isEdit ? 'Сохранить изменения' : 'Сохранить'}</button></div></form>`;
    dialog.dataset.section = sectionId;
    dialog.dataset.id = row?.id || '';
    dialog.dataset.custom = custom?.kind || '';
    dialog.showModal();
    document.getElementById('dataForm').addEventListener('submit', event => submitForm(event, sectionId, row, custom));
}
function collectFormData(fields, sectionId, isEdit) {
    const dto = {};
    for (const field of fields) {
        if (field.createOnly && isEdit) continue;
        const input = document.getElementById(`field-${field.key}`);
        if (!input || input.readOnly) continue;
        let value = input.value;
        if (field.type === 'number') value = value === '' ? 0 : Number(value);
        else if (field.type === 'animals' || field.type === 'products') value = value ? Number(value) : null;
        else if (field.type === 'date' && !value) value = null;
        dto[field.key] = value;
    }
    return dto;
}
async function submitForm(event, sectionId, row, custom) {
    event.preventDefault();
    const form = event.currentTarget;
    if (!form.reportValidity()) return;
    const fields = custom?.fields || sections[sectionId].fields;
    const data = collectFormData(fields, sectionId, Boolean(row));
    const formError = document.getElementById('formError');
    const saveButton = form.querySelector('button[type="submit"]');
    saveButton.disabled = true;
    saveButton.textContent = 'Сохраняем…';
    try {
        if (custom?.kind === 'consume') {
            await request(`/${sections.feeds.endpoint}/${custom.targetId}/consume`, { method: 'POST', body: JSON.stringify(data) });
        } else {
            const method = row ? 'PUT' : 'POST';
            const endpoint = sections[sectionId].endpoint;
            await request(`/${endpoint}${row ? `/${row.id}` : ''}`, { method, body: JSON.stringify(data) });
        }
        document.getElementById('formDialog').close();
        showToast(custom?.kind === 'consume' ? 'Расход корма записан' : row ? 'Изменения сохранены' : sectionId === 'sales' ? 'Продажа оформлена' : 'Запись добавлена');
        await navigate();
    } catch (error) {
        formError.textContent = error.message || 'Не удалось сохранить данные';
        saveButton.disabled = false;
        saveButton.innerHTML = `${icon('plus')}${row ? 'Сохранить изменения' : 'Сохранить'}`;
    }
}
async function deleteRow(id) {
    const section = sections[activeSection];
    const row = (datasets[activeSection] || []).find(item => String(item.id) === String(id));
    const name = row?.name || row?.userName || row?.animalName || row?.productName || section.singular;
    const question = activeSection === 'accounts' ? `Удалить учётную запись работника «${name}»?` : `Удалить «${name}»? Это действие нельзя отменить.`;
    if (!window.confirm(question)) return;
    try {
        await request(`/${section.endpoint}/${id}`, { method: 'DELETE' });
        showToast('Запись удалена');
        await navigate();
    } catch (error) { showToast(`Не удалось удалить: ${error.message}`, 'error'); }
}
function openConsumeForm(id) {
    const feed = (datasets.feeds || []).find(item => item.id === Number(id));
    if (!feed) return;
    const fields = [
        { key: 'quantity', label: `Количество для списания (${labelFor('unit', feed.unit)})`, type: 'number', required: true, min: 0.01, step: 0.1 },
        { key: 'date', label: 'Дата расхода', type: 'date', required: true, default: today },
        { key: 'notes', label: 'Комментарий', max: 200 }
    ];
    openForm('feeds', null, { title: `Списание: ${feed.name}`, fields, kind: 'consume', targetId: feed.id });
}
document.addEventListener('click', event => {
    const button = event.target.closest('[data-action]');
    if (!button) return;
    const action = button.dataset.action;
    if (action === 'create' || action === 'create-animal') {
        const id = action === 'create-animal' ? 'animals' : activeSection;
        openForm(id);
    } else if (action === 'edit') {
        const row = (datasets[activeSection] || []).find(item => item.id === Number(button.dataset.id));
        if (row) openForm(activeSection, row);
    } else if (action === 'delete') deleteRow(button.dataset.id);
    else if (action === 'consume') openConsumeForm(button.dataset.id);
    else if (action === 'close-modal') document.getElementById('formDialog').close();
});
document.addEventListener('input', event => { if (event.target.id === 'tableSearch') applySearch(event.target.value); });
window.addEventListener('hashchange', () => { if (authUser) navigate(); });
startApp();
