/*// wwwroot/js/allUsers.js
async function fetchUsers() {
    try {
        const response = await fetch('/Admin/Employee/AllUsers');
        const data = await response.json();
        const users = data.users;

        const tbody = document.querySelector('tbody');
        tbody.innerHTML = '';

        users.forEach(user => {
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${user.id}</td>
                <td>${user.fullName}</td>
                <td>${user.email}</td>
                <td>${user.roles.join(', ')}</td>
                <td>
                    <!-- Add buttons for giving employee roles here -->
                </td>
                <td>
                    <!-- Add form for adding money here -->
                </td>
                <td>
                    <!-- Add form for deleting user here -->
                </td>
            `;
            tbody.appendChild(tr);
        });
    } catch (error) {
        console.error('Error fetching users:', error);
    }
}

fetchUsers();
*/
document.addEventListener('DOMContentLoaded', function () {
    fetch('https://localhost:7160/Admin/Employee/AllUsersJson')
        .then(response => {
            if (!response.ok) {
                console.log('Failed to fetch data');
            }
            console.log(response.json());
            return response.json();
        })
        .then(data => {
            data.users.forEach(user => {
                const row = `
                    <tr>
                        <td>${user.id}</td>
                        <td>${user.fullName}</td>
                        <td>${user.email}</td>
                    </tr>`;
                document.getElementById('userTableBody').insertAdjacentHTML('beforeend', row);
            });
        })
        .catch(error => {
            console.log(error.message); // Corrected line
        });
});
