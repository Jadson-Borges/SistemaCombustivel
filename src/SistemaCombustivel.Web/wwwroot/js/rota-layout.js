(() => {
    const raiz = document.documentElement;

    const atualizarBotaoDeTema = () => {
        const escuro = raiz.getAttribute('data-theme') === 'dark';
        document.querySelectorAll('[data-theme-toggle]').forEach(botao => {
            const icone = botao.querySelector('.theme-toggle-icon');
            const rotulo = botao.querySelector('.theme-toggle-label');
            if (icone) icone.className = escuro ? 'bi bi-sun-fill theme-toggle-icon' : 'bi bi-moon-stars-fill theme-toggle-icon';
            if (rotulo) rotulo.textContent = escuro ? 'Tema claro' : 'Tema escuro';
        });
    };

    const fecharMenu = () => {
        document.querySelector('.app-sidebar')?.classList.remove('is-open');
        raiz.classList.remove('rota-menu-open');
    };

    // UM listener no document: vale para botões que o Blazor criar depois.
    document.addEventListener('click', (evento) => {
        const alvo = evento.target.closest(
            '[data-theme-toggle], [data-sidebar-collapse], [data-sidebar-open], [data-sidebar-close], .main-nav-link');
        if (!alvo) return;

        if (alvo.matches('[data-theme-toggle]')) {
            const novo = raiz.getAttribute('data-theme') === 'dark' ? 'light' : 'dark';
            raiz.setAttribute('data-theme', novo);
            try { localStorage.setItem('rota-tema', novo); } catch { }
            atualizarBotaoDeTema();
        } else if (alvo.matches('[data-sidebar-collapse]')) {
            const recolhida = raiz.classList.toggle('rota-sidebar-collapsed');
            try { localStorage.setItem('rota-sidebar', recolhida ? 'recolhida' : 'expandida'); } catch { }
        } else if (alvo.matches('[data-sidebar-open]')) {
            document.querySelector('.app-sidebar')?.classList.add('is-open');
            raiz.classList.add('rota-menu-open');
        } else {
            fecharMenu(); // data-sidebar-close ou clique num link do menu
        }
    });

    atualizarBotaoDeTema();
    // A navegação "enhanced" do Blazor troca partes do DOM sem recarregar a página.
    window.Blazor?.addEventListener('enhancedload', atualizarBotaoDeTema);
})();