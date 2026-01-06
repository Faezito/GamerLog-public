    feather.replace();

    const openBtn = document.getElementById('abrir-camera');
    const telaPrincipal = document.getElementById('start-screen');
    const cameraScreen = document.getElementById('camera-screen');
    const video = document.getElementById('video');
    const canvas = document.getElementById('canvas');
    const takeBtn = document.getElementById('take-photo');
    const closeBtn = document.getElementById('close-camera');
    const switchBtn = document.getElementById('switch-camera');
    const photoActions = document.getElementById('photo-actions');
    const saveBtn = document.getElementById('save-photo');
    const discardBtn = document.getElementById('discard-photo');
    const inputImg = document.getElementById('imagem64');
    const caminhoImagem = document.getElementById('caminho-imagem');
    let imagem = null;
    const preview = document.getElementById('preview-imagem');

    let stream;
    let usingFrontCamera = false;

    //async function iniciarCamera() {
    //  if (stream) {
    //    stream.getTracks().forEach(track => track.stop());
    //  }

    //  const constraints = {
    //    video: { facingMode: usingFrontCamera ? 'user' : 'environment' }
    //  };

    //  try {
    //    stream = await navigator.mediaDevices.getUserMedia(constraints);
    //    video.srcObject = stream;
    //  } catch (e) {
    //    alert('Erro ao acessar a câmera: ' + e.message);
    //  }

    //  $('#mensagem').html("")
    //  $('#numeroDoc').text('')
//}

async function iniciarCamera() {
    if (stream) {
        stream.getTracks().forEach(track => track.stop());
    }

    let constraintsList = [
        { video: { facingMode: { exact: usingFrontCamera ? "user" : "environment" } } },
        { video: { facingMode: usingFrontCamera ? "user" : "environment" } },
        { video: true }
    ];

    for (let c of constraintsList) {
        try {
            stream = await navigator.mediaDevices.getUserMedia(c);
            video.srcObject = stream;
            break;
        } catch (e) {
            console.log("Falhou com: ", c, e.message);
        }
    }

    if (!stream) {
        alert("Erro ao acessar a câmera. No iPhone, verifique: Ajustes → Safari → Câmera → Permitir");
        return;
    }

    $('#mensagem').html("");
    $('#numeroDoc').text("");
}

    openBtn.addEventListener('change', async (e) => {
        if (confirm('Deseja abrir a câmera?')){
        telaPrincipal.style.display = 'none';
        cameraScreen.style.display = 'flex';
        photoActions.style.display = 'none';
        document.getElementById('camera-controls').style.display = 'flex';
        //$('#ProtheusID').val(e.target.value)
        await iniciarCamera();
        } else {
        e.target.value = '1';
        }
    });

    // Fechar
    closeBtn.addEventListener('click', () => {
        if (stream) stream.getTracks().forEach(track => track.stop());
        video.srcObject = null;
        cameraScreen.style.display = 'none';
        telaPrincipal.style.display = '';
        canvas.style.display = 'none';
        video.style.display = 'block';
        openBtn.value = '1';
        location.reload()
    });

    // Alternar camera
    switchBtn.addEventListener('click', async () => {
      usingFrontCamera = !usingFrontCamera;
      video.style.display = 'block';
      await iniciarCamera();
    });

    // tirar foto
    takeBtn.addEventListener('click', () => {
      canvas.width = video.videoWidth;
      canvas.height = video.videoHeight;
      const ctx = canvas.getContext('2d');
      ctx.drawImage(video, 0, 0, canvas.width, canvas.height);

      video.style.display = 'none';
      canvas.style.display = 'block';
      document.getElementById('camera-controls').style.display = 'none';
      photoActions.style.display = 'flex';
      //menuOpcoes.style.display = 'flex';
    });

    // Descartar
    discardBtn.addEventListener('click', () => {
        canvas.style.display = 'none';
        video.style.display = 'block';
        photoActions.style.display = 'none';
        $('#mensagem').html("<span style='Display: none;'></span>");
        $('#btn-Salvar').css('display', 'none');
      document.getElementById('camera-controls').style.display = 'flex';
    });




