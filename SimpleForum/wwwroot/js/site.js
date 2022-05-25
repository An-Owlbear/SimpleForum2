const toggleDropdown = () => {
    const dropdown = document.querySelector('.profile-dropdown');
    if (dropdown.classList.contains('hidden')) dropdown.classList.remove('hidden');
    else dropdown.classList.add('hidden');
}