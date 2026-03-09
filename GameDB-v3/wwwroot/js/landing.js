/**
 * GAMERLOG — landing.js
 * Arquivo: wwwroot/js/landing.js
 *
 * Módulos:
 *  1. Cursor personalizado
 *  2. XP bar animada
 *  3. Data no HUD mockup
 *  4. Achievement toast
 *  5. Scroll reveal
 *  6. Pixel canvas (CTA background)
 *  7. Form login (AJAX + SweetAlert2)
 *  8. Form recuperar senha (AJAX + SweetAlert2)
 */

(function () {
    'use strict';

    /* ─────────────────────────────────────────
       1. CURSOR PERSONALIZADO
    ───────────────────────────────────────── */
    const cursor = document.getElementById('cursor');
    const ring   = document.getElementById('cursor-ring');

    if (cursor && ring) {
        let mx = 0, my = 0, rx = 0, ry = 0;

        document.addEventListener('mousemove', function (e) {
            mx = e.clientX;
            my = e.clientY;
        });

        function animateCursor() {
            cursor.style.left = mx + 'px';
            cursor.style.top  = my + 'px';

            // Anel com lag suave
            rx += (mx - rx) * 0.12;
            ry += (my - ry) * 0.12;
            ring.style.left = rx + 'px';
            ring.style.top  = ry + 'px';

            requestAnimationFrame(animateCursor);
        }

        animateCursor();
    }

    /* ─────────────────────────────────────────
       2. XP BAR ANIMADA
    ───────────────────────────────────────── */
    const xpFill  = document.getElementById('xpFill');
    const xpCount = document.getElementById('xpCount');

    if (xpFill && xpCount) {
        const XP_TARGET     = 730;
        const XP_TOTAL      = 1000;
        const XP_FILL_PCT   = (XP_TARGET / XP_TOTAL * 100).toFixed(0) + '%';
        const TICK_INTERVAL = 40; // ms
        const TICK_STEP     = 12;

        setTimeout(function () {
            xpFill.style.width = XP_FILL_PCT;

            let current = 0;
            const interval = setInterval(function () {
                current = Math.min(current + TICK_STEP, XP_TARGET);
                xpCount.textContent = current + ' / ' + XP_TOTAL + ' XP';
                if (current >= XP_TARGET) clearInterval(interval);
            }, TICK_INTERVAL);
        }, 800);
    }

    /* ─────────────────────────────────────────
       3. DATA NO HUD MOCKUP
    ───────────────────────────────────────── */
    const hudDate = document.getElementById('hudDate');
    if (hudDate) {
        hudDate.textContent = new Date().toLocaleDateString('pt-BR');
    }

    /* ─────────────────────────────────────────
       4. ACHIEVEMENT TOAST
    ───────────────────────────────────────── */
    const achToast = document.getElementById('achToast');

    if (achToast) {
        const SHOW_DELAY = 2800;
        const HIDE_DELAY = 4500;

        setTimeout(function () {
            achToast.classList.add('show');
            setTimeout(function () {
                achToast.classList.remove('show');
            }, HIDE_DELAY);
        }, SHOW_DELAY);
    }

    /* ─────────────────────────────────────────
       5. SCROLL REVEAL
    ───────────────────────────────────────── */
    const revealEls = document.querySelectorAll('.reveal');

    if (revealEls.length > 0 && 'IntersectionObserver' in window) {
        const observer = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting) {
                    entry.target.classList.add('visible');
                }
            });
        }, { threshold: 0.1 });

        revealEls.forEach(function (el) {
            observer.observe(el);
        });
    } else {
        // Fallback: revela tudo imediatamente
        revealEls.forEach(function (el) { el.classList.add('visible'); });
    }

    /* ─────────────────────────────────────────
       6. PIXEL CANVAS (CTA background)
    ───────────────────────────────────────── */
    const cvs = document.getElementById('pixelCanvas');

    if (cvs) {
        const ctx = cvs.getContext('2d');
        const PIXEL_SIZE = 16;

        function resizeCanvas() {
            cvs.width  = cvs.offsetWidth;
            cvs.height = cvs.offsetHeight;
            drawPixels();
        }

        function drawPixels() {
            ctx.clearRect(0, 0, cvs.width, cvs.height);

            for (let x = 0; x < cvs.width; x += PIXEL_SIZE) {
                for (let y = 0; y < cvs.height; y += PIXEL_SIZE) {
                    if (Math.random() > 0.88) {
                        const alpha = Math.random() * 0.5 + 0.1;
                        ctx.fillStyle = Math.random() > 0.5
                            ? 'rgba(6,182,212,' + alpha + ')'
                            : 'rgba(37,99,235,' + alpha + ')';
                        ctx.fillRect(x, y, PIXEL_SIZE - 1, PIXEL_SIZE - 1);
                    }
                }
            }
        }

        window.addEventListener('resize', resizeCanvas);
        resizeCanvas();
    }

    /* ─────────────────────────────────────────
       7. FORM LOGIN (AJAX)
       Depende de: jQuery, SweetAlert2 (Swal)
    ───────────────────────────────────────── */
    $('#form-login').on('submit', function (e) {
        e.preventDefault();

        const $btn      = $('#btnLogin');
        const $spinner  = $('#loading-login');
        const $label    = $('#texto-login');

        $.ajax({
            url: '/Home/Login',
            type: 'POST',
            data: $(this).serialize(),
            headers: { 'X-Requested-With': 'XMLHttpRequest' },

            beforeSend: function () {
                $btn.prop('disabled', true);
                $spinner.removeClass('d-none');
                $label.hide();
            },

            complete: function () {
                $btn.prop('disabled', false);
                $spinner.addClass('d-none');
                $label.show();
            },

            success: function (response) {
                if (response.success) {
                    window.location.href = response.redirectUrl;
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: response.title  ?? 'Erro',
                        text:  response.detail
                    });
                }
            },

            error: function (xhr) {
                const json = xhr.responseJSON;
                Swal.fire({
                    icon:  'error',
                    title: json?.title  ?? 'Erro',
                    text:  json?.detail ?? 'Ocorreu um erro inesperado.'
                });
            }
        });
    });

    /* ─────────────────────────────────────────
       8. FORM RECUPERAR SENHA (AJAX)
       Depende de: jQuery, SweetAlert2 (Swal)
    ───────────────────────────────────────── */
    $(document).on('click', '#recuperar-senha', function (e) {
        e.preventDefault();

        Swal.fire({
            title: 'Realmente deseja resetar sua senha? Esta ação é irreversível.',
            showDenyButton:  true,
            denyButtonText:  'Não',
            confirmButtonText: 'Sim'
        }).then(function (result) {
            if (!result.isConfirmed) return;

            const $btn     = $('#recuperar-senha');
            const $spinner = $('#loading-recuperar');
            const $label   = $('#texto-salvar');

            $.ajax({
                url: '/Home/RecuperarSenha',
                type: 'POST',
                data: $('#form-recuperar').serialize(),
                headers: { 'X-Requested-With': 'XMLHttpRequest' },

                beforeSend: function () {
                    $btn.prop('disabled', true);
                    $spinner.removeClass('d-none');
                    $label.hide();
                },

                complete: function () {
                    $btn.prop('disabled', false);
                    $spinner.addClass('d-none');
                    $label.show();
                }
            })
            .done(function (response) {
                if (response.success) {
                    window.location.href = response.redirectUrl;
                }
            })
            .fail(function (xhr) {
                if (xhr.responseJSON) {
                    Swal.fire({
                        icon:  'error',
                        title: xhr.responseJSON.title  ?? 'Erro',
                        html:  xhr.responseJSON.detail
                    });
                } else {
                    Swal.fire({
                        icon:  'error',
                        title: 'Erro',
                        text:  'Erro inesperado ao processar a requisição.'
                    });
                }
            });
        });
    });

})();
