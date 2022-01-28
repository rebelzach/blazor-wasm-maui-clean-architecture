import * as THREE from './three.js/three.module.js';

import Stats from './three.js/stats.module.js';

import { STLLoader } from './three.js/STLLoader.js';

export function solidModelMeshView(container) {
    //let stats
    let mesh;

    let camera, cameraTarget, scene, renderer;
    const width = 400;
    const height = 400;

    const loader = init();
    animate();

    return {
        loadAsciiText: loader,
        render: render
    }

    function init() {

        camera = new THREE.PerspectiveCamera(35, width / height, 1, 15);
        camera.position.set(3, 3, 3);

        cameraTarget = new THREE.Vector3(0, - 0.25, 0);

        scene = new THREE.Scene();
        scene.background = new THREE.Color(0x72645b);
        //scene.fog = new THREE.Fog(0x72645b, 2, 15);

        // Ground

        //const plane = new THREE.Mesh(
        //    new THREE.PlaneGeometry(40, 40),
        //    new THREE.MeshPhongMaterial({ color: 0x999999, specular: 0x101010 })
        //);
        //plane.rotation.x = - Math.PI / 2;
        //plane.position.y = - 0.5;
        //scene.add(plane);

        //plane.receiveShadow = true;

        // Lights

        scene.add(new THREE.HemisphereLight(0x443333, 0x111122));

        addShadowedLight(1, 1, 1, 0xffffff, 1.35);
        addShadowedLight(0.5, 1, - 1, 0xffaa00, 1);
        // renderer

        renderer = new THREE.WebGLRenderer({ antialias: true });
        renderer.setPixelRatio(window.devicePixelRatio);
        renderer.setSize(width, height);
        renderer.outputEncoding = THREE.sRGBEncoding;

        renderer.shadowMap.enabled = true;

        container.appendChild(renderer.domElement);

        // stats

        //stats = new Stats();
        //container.appendChild(stats.dom);

        //

        window.addEventListener('resize', onWindowResize);

        // ASCII data
        const loader = new STLLoader();
        return function (asciiText) {
            let geometry = loader.parse(asciiText);

            const material = new THREE.MeshPhongMaterial({ color: 0xff5533, specular: 0x111111, shininess: 200 });
            if (mesh)
                scene.remove(mesh);
            mesh = new THREE.Mesh(geometry, material);

            mesh.scale.set(0.01, 0.01, 0.01);
            mesh.position.set(0, - 0.25, -0.5);
            mesh.rotation.set(- Math.PI, - Math.PI, - Math.PI);

            //mesh.castShadow = true;
            //mesh.receiveShadow = true;

            scene.add(mesh);

            //render();
        };
    }

    function addShadowedLight(x, y, z, color, intensity) {

        const directionalLight = new THREE.DirectionalLight(color, intensity);
        const light: any = directionalLight;
        light.position.set(x, y, z);
        scene.add(directionalLight);

        directionalLight.castShadow = true;

        const d = 1;
        directionalLight.shadow.camera.left = - d;
        directionalLight.shadow.camera.right = d;
        directionalLight.shadow.camera.top = d;
        directionalLight.shadow.camera.bottom = - d;

        directionalLight.shadow.camera.near = 1;
        directionalLight.shadow.camera.far = 4;

        directionalLight.shadow.bias = - 0.002;

    }

    function onWindowResize() {

        camera.aspect = window.innerWidth / window.innerHeight;
        camera.updateProjectionMatrix();

        renderer.setSize(window.innerWidth, window.innerHeight);

    }

    function animate() {

        setTimeout(function () {

            requestAnimationFrame(animate);

        }, 1000 / 10);

        render();
    }

    function render() {

        const timer = Date.now() * 0.0005;

        camera.position.x = Math.cos(timer) * 3;
        camera.position.z = Math.sin(timer) * 3;

        camera.lookAt(cameraTarget);

        renderer.render(scene, camera);
    }
}